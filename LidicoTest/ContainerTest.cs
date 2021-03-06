﻿using Lidico;
using LidicoTest.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LidicoTest
{
    [TestClass]
    public class ContainerTest
    {
        private Container _container;

        [TestInitialize]
        public void TestInitialise()
        {
            _container = new Container();
        }

        [TestMethod]
        public void TestWithoutContainer()
        {
            //Arrange
            var visa = new Visa();
            var master = new Master();
            var visaCustomer = new Customer(visa);
            var masterCustomer = new Customer(master);

            //Act/Assert
            Assert.AreEqual("Visa card charged with 5.", visaCustomer.Charge(5));
            Assert.AreEqual("Visa card refunded with 5.", visaCustomer.Refund(5));
            Assert.AreEqual("MasterCard charged with 10.", masterCustomer.Charge(10));
            Assert.AreEqual("MasterCard refunded with 10.", masterCustomer.Refund(10));
        }

        [TestMethod]
        public void Container_Register_Resolve_Object()
        {
            //Arrange
            _container.Register<Customer, Customer>();
            _container.Register<IDebitCard, Visa>();

            //Act
            var visaCustomer = _container.Resolve<Customer>();
            //Assert
            Assert.AreEqual("Visa card charged with 5.", visaCustomer.Charge(5));
            Assert.AreEqual("Visa card refunded with 5.", visaCustomer.Refund(5));
        }

        [TestMethod]
        public void Container_ReRegister_Resolve_Object()
        {
            //Arrange
            _container.Register<Customer, Customer>();
            _container.Register<IDebitCard, Visa>();
            //Reregister
            _container.Register<IDebitCard, Master>();
            //Act
            var masterCustomer = _container.Resolve<Customer>();

            //Assert
            Assert.AreEqual("MasterCard charged with 10.", masterCustomer.Charge(10));
            Assert.AreEqual("MasterCard refunded with 10.", masterCustomer.Refund(10));
        }

        [ExpectedException(typeof(UnassignableException))]
        [TestMethod]
        public void Container_RegisterWithWrongType_Exception()
        {
            //Arrange
            _container.Register<Master, Customer>();
            _container.Register<IDebitCard, Visa>();
            //Act
            var visaCustomer = _container.Resolve<Customer>();
            //Assert
            Assert.AreEqual("Visa card charged with 5.", visaCustomer.Charge(5));
            Assert.AreEqual("Visa card refunded with 5.", visaCustomer.Refund(5));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _container = null;
        }
    }
}