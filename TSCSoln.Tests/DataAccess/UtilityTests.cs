using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.DataAccess;
using TSCSoln.Entities.Test;

namespace TSCSoln.Tests.DataAccess
{
    [TestClass]
    public class UtilityTests
    {
        private static DataTable CreateTestTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SQ_TEST_ID", typeof(int));
            dt.Columns.Add("CH_TEST_NAME", typeof(string));
            return dt;
        }

        [TestMethod]
        public void DataTableToList_EmptyTable_ReturnsEmptyList()
        {
            var result = Utility.DataTableToList<GetTestDTO>(CreateTestTable());

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DataTableToList_TableWithoutColumns_ReturnsEmptyList()
        {
            var result = Utility.DataTableToList<GetTestDTO>(new DataTable());

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DataTableToList_MapsMatchingColumnsToProperties()
        {
            DataTable dt = CreateTestTable();
            dt.Rows.Add(42, "Alpha");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(42, result[0].SQ_TEST_ID);
            Assert.AreEqual("Alpha", result[0].CH_TEST_NAME);
        }

        [TestMethod]
        public void DataTableToList_MultipleRows_PreservesRowOrder()
        {
            DataTable dt = CreateTestTable();
            dt.Rows.Add(1, "First");
            dt.Rows.Add(2, "Second");
            dt.Rows.Add(3, "Third");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result.ConvertAll(x => x.SQ_TEST_ID));
            CollectionAssert.AreEqual(new[] { "First", "Second", "Third" }, result.ConvertAll(x => x.CH_TEST_NAME));
        }

        [TestMethod]
        public void DataTableToList_ReturnsDistinctInstancePerRow()
        {
            DataTable dt = CreateTestTable();
            dt.Rows.Add(1, "A");
            dt.Rows.Add(2, "B");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreNotSame(result[0], result[1]);
        }

        [TestMethod]
        public void DataTableToList_IgnoresColumnsWithoutMatchingProperty()
        {
            DataTable dt = CreateTestTable();
            dt.Columns.Add("UNKNOWN_COLUMN", typeof(string));
            dt.Rows.Add(7, "Beta", "ignored");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0].SQ_TEST_ID);
            Assert.AreEqual("Beta", result[0].CH_TEST_NAME);
        }

        [TestMethod]
        public void DataTableToList_PropertyWithoutMatchingColumn_KeepsDefaultValue()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CH_TEST_NAME", typeof(string));
            dt.Rows.Add("OnlyName");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0].SQ_TEST_ID);
            Assert.AreEqual("OnlyName", result[0].CH_TEST_NAME);
        }

        [TestMethod]
        public void DataTableToList_ColumnNameMatchingIsCaseSensitive()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sq_test_id", typeof(int));
            dt.Columns.Add("ch_test_name", typeof(string));
            dt.Rows.Add(5, "Lower");

            var result = Utility.DataTableToList<GetTestDTO>(dt);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0].SQ_TEST_ID);
            Assert.IsNull(result[0].CH_TEST_NAME);
        }

        // Characterization test: documents current behavior. A NULL database value
        // (DBNull) cannot be assigned to a string property and reflection throws.
        [TestMethod]
        public void DataTableToList_DbNullValue_ThrowsArgumentException()
        {
            DataTable dt = CreateTestTable();
            dt.Rows.Add(1, DBNull.Value);

            Assert.ThrowsException<ArgumentException>(() => Utility.DataTableToList<GetTestDTO>(dt));
        }
    }
}
