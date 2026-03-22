using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR12;
using System.Linq;

namespace PR12.CaptchaTests
{
    [TestClass]
    public class CaptchaUnitTest
    {
        [TestMethod]
        public void RegisterFailedPasswordAttempt_ChangesLastFailedLoginOnlyWhenDifferentUser()
        {
            var logic = new CaptchaLogic();

            logic.RegisterFailedPasswordAttempt("vickss");
            Assert.AreEqual("vickss", logic.lastFailedLogin);

            logic.RegisterFailedPasswordAttempt("vickss");
            Assert.AreEqual("vickss", logic.lastFailedLogin, "Должно остаться то же имя");

            logic.RegisterFailedPasswordAttempt("sonyanya");
            Assert.AreEqual("sonyanya", logic.lastFailedLogin, "Должно обновиться при смене пользователя");
        }

        [TestMethod]
        public void ShouldShowCaptcha_BecomesTrueExactlyOnThirdAttempt()
        {
            var logic = new CaptchaLogic();

            Assert.IsFalse(logic.ShouldShowCaptcha(), "0 попыток → капча не показывается");

            logic.RegisterFailedPasswordAttempt("vickss");
            Assert.IsFalse(logic.ShouldShowCaptcha(), "1 попытка");

            logic.RegisterFailedPasswordAttempt("vickss");
            Assert.IsFalse(logic.ShouldShowCaptcha(), "2 попытки");

            logic.RegisterFailedPasswordAttempt("vickss");
            Assert.IsTrue(logic.ShouldShowCaptcha(), "3 попытки → теперь капча показывается");
        }

        [TestMethod]
        public void ShouldShowCaptcha_AfterTwoFailedAttempts_ReturnsFalse()
        {
            var logic = new CaptchaLogic();

            logic.RegisterFailedPasswordAttempt("vill");
            logic.RegisterFailedPasswordAttempt("vill");

            Assert.IsFalse(logic.ShouldShowCaptcha());
        }

        [TestMethod]
        public void GenerateCaptchaText_ContainsConfusingCharacters_ShouldFailBy1()
        {
            var logic = new CaptchaLogic();
            string confusing = "O0Il1";

            for (int i = 0; i < 200; i++)
            {
                logic.GenerateCaptchaText();
                foreach (char bad in confusing)
                {
                    if (logic.captchaText.Contains(bad.ToString()))
                    {
                        Assert.Fail($"Капча содержит неоднозначный символ '{bad}' → {logic.captchaText}");
                    }
                }
            }
        }

        [TestMethod]
        public void GenerateCaptchaText_ShouldContainDigitAndSpecialChar_FailInMostCases()
        {
            var logic = new CaptchaLogic();
            int attempts = 100;
            int missingDigit = 0;
            int missingSpecial = 0;
            string specialChars = "!@#$%^&*";

            for (int i = 0; i < attempts; i++)
            {
                logic.GenerateCaptchaText();
                string t = logic.captchaText;

                if (!t.Any(char.IsDigit)) missingDigit++;

                if (!t.Any(c => specialChars.Contains(c))) missingSpecial++;
            }

            Assert.IsTrue(missingDigit <= attempts * 0.25,
                $"В {missingDigit} из {attempts} капч нет ни одной цифры — это слишком часто");

            Assert.IsTrue(missingSpecial <= attempts * 0.50,
                $"В {missingSpecial} из {attempts} капч нет ни одного спецсимвола — это слишком часто");
        }

        [TestMethod]
        public void IsCaptchaCorrect_CaseSensetiveCaptcha_ShouldFail()
        {
            var logic = new CaptchaLogic();
            logic.captchaText = "AbCdEf";

            Assert.IsTrue(logic.IsCaptchaCorrect("abcdef"), "Капча чувствительна к регистру"); 
        }

        [TestMethod]
        public void IsCaptchaCorrect_WrongUserInput_ReturnsFalse()
        {
            var logic = new CaptchaLogic();
            logic.captchaText = "XyZ123";

            Assert.IsFalse(logic.IsCaptchaCorrect("wrong"));
        }

        [TestMethod]
        public void GenerateCaptchaText_CreatesNewTextAfterReset_ReturnsTrue()
        {
            var logic = new CaptchaLogic();
            logic.GenerateCaptchaText();
            string old = logic.captchaText;

            logic.Reset();
            logic.GenerateCaptchaText();
            string now = logic.captchaText;

            Assert.AreNotEqual(old, now);
        }
    }
}
