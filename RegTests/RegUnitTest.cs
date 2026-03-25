using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR12;

namespace RegTests
{
    [TestClass]
    public class RegUnitTest
    {
        [TestMethod]
        public void RegTest_Success()
        {
            var regPage = new registrationPage();
            Assert.IsTrue(regPage.Reg("ruslanchik", "aipadKid4"));
        }

        [TestMethod]
        public void RegTest_Fail()
        {
            var regPage = new registrationPage();

            Assert.IsFalse(regPage.Reg("vickss", "anyPassword123")); //проверка регистрации уже существующего пользователя

            Assert.IsFalse(regPage.Reg("", "")); //проверка входа со всеми пустыми полями
            Assert.IsFalse(regPage.Reg("", "Password123")); //проверка входа с пустым полем логина
            Assert.IsFalse(regPage.Reg("newuser_empty_pass", ""));  //проверка входа с пустым полем пароля

            Assert.IsFalse(regPage.Reg("a", "Password123")); //проверка ввода логина меньше 2 символов
            Assert.IsFalse(regPage.Reg("validuser", "1234")); //проверка ввода пароля меньше 5 символов

            Assert.IsFalse(regPage.Reg("validuser", "OnlyLetters")); //проверка ввода пароля без цифр
        }
    }
}
