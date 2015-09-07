using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenericList.Tests
{
    [TestClass]
    public class GenericListTests
    {
        private GenericList<int> list;

        [TestInitialize]
        public void TestInit()
        {
            list = new GenericList<int>();
        }

        [TestMethod]
        public void Adding_New_Element_Should_Increase_Count_By_1()
        {            
            list.Add(5);

            Assert.AreEqual(1, list.Count, "Count did not increment.");
        }

        [TestMethod]
        public void Adding_17_Elements_Should_Resize_Correctly()
        {
            for (int i = 0; i < 17; i++)
            {
                list.Add(i);
            }

            Assert.AreEqual(17, list.Count);
        }

        [TestMethod]
        [Ignore]
        public void Find_on_Existing_Element_Should_Return_its_Index_In_Ordered_Collection()
        {
            for (int i = 0; i < 17; i++)
            {
                list.Add(i);
            }

            var index = list.Find(5);
            Assert.AreEqual(5, index);
        }

        [TestMethod]
        public void Find_on_Existing_Element_Should_Return_its_Index_In_Unordered_Collection()
        {
            list.Add(5);
            list.Add(1);
            list.Add(4);
            list.Add(10);
            list.Add(5);
            list.Add(1);
            list.Add(4);
            list.Add(5);
            list.Add(1);
            list.Add(4);
            list.Add(3);

            var index = list.Find(10);
            Assert.AreEqual(3, index);           
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Removing_Element_At_Invalid_Index_Should_Throw_Exception()
        {
            list.RemoveAt(1);
        }
    }
}
