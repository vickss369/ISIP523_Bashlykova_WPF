using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR12;
using System.Windows.Controls;

namespace AuthTests
{
    [TestClass]
    public class UnitTest1
    {
        /*[TestMethod]
        public void AuthTest()
        {
            var page = new registrationPage();
            Assert.IsTrue(page.Auth("test", "test"));
            Assert.IsFalse(page.Auth("user1", "12345"));
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth(" ", " "));
        }*/

        [TestMethod]
        public void AuthTest_Success()
        {
            var authPage = new registrationPage();
            Assert.IsTrue(authPage.Auth("vickss", "vikaaa3"));
            Assert.IsTrue(authPage.Auth("ami", "amishers7"));
            Assert.IsTrue(authPage.Auth("sonyanya", "krokikk5"));
            Assert.IsTrue(authPage.Auth("vill", "villyalyalya123"));

            Assert.IsTrue(authPage.Auth("VICKSS", "vikaaa3")); //проверка независимости логина от регистра: ввод в верхнем регистре
            Assert.IsTrue(authPage.Auth("vickss", "vikaaa3")); //проверка независимости логина от регистра: ввод в нижнем регистре
        }

        [TestMethod]
        public void AuthTest_Fail() 
        {
            var authPage = new registrationPage();

            Assert.IsFalse(authPage.Auth("vickss", "VIKAAA3")); //проверка ввода пароля в строго заданном регистре

            Assert.IsFalse(authPage.Auth("vickss", "villyalyalya123")); //проверка ввода пароля, соответствующего другому пользователю

            Assert.IsFalse(authPage.Auth("user1", "12345")); //проверка входа со случайными данными пользователя, которого нет в БД

            Assert.IsFalse(authPage.Auth("", "")); //проверка входа со всеми пустыми полями
            Assert.IsFalse(authPage.Auth("vickss", "")); //проверка входа с пустым полем пароля
            Assert.IsFalse(authPage.Auth("", "vikaaa3")); //проверка входа с пустым полем логина
        }
    }
}
