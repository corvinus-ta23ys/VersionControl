﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidateEmail(email);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [
            Test,
            TestCase("KISkutya", false),
            TestCase("NAGYBETUS", false),
            TestCase("kisbetus", false),
            TestCase("roVID12", false),
            TestCase("Megfelelo123", true)
        ]

        public void TestValidatePassword(string password, bool expectedResult)
        {
            var kisbetu = new Regex(@"[a-z]+");
            var nagybetu = new Regex(@"[A-Z]+");
            var szam = new Regex(@"[0-9]+");
            var hossz = new Regex(@".{8,}");
            bool igaze= kisbetu.IsMatch(password) && nagybetu.IsMatch(password) && szam.IsMatch(password) && hossz.IsMatch(password);
            Assert.AreEqual(expectedResult, igaze);
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567"),
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }
    }
}
