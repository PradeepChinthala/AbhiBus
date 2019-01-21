using System;
using System.Collections.Generic;
using AventStack.ExtentReports;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using Planit.Automation.Parameters;
using Planit.Automation.Selenium;
using Planit.Automation.TestCases;

namespace Planit.Automation.TeseCases
{
    [TestFixture]
    public class Test : Common
    {

        // Runstep is a method which handle the exception uisng try,catch will have 2 paramters as shown below and it will log the step information on console
        /// <summary>
        /// <param name="MethodName"></param>
        /// /// <param name="Step Information"></param>
        /// </summary>
        
        [Test]
        public void TestCase()
        {
            // MouseOver To Tool tip and Validating with Expected ToolTip text
            RunStep(() => { HomePage.MoveGetToolTip().Should().Equals(Parameter.Get<string>("TooTip")); },
            "Hover and Verify Tool tip");  

            //Enter Search criteria 
            RunStep(HomePage.EnterSearch, Parameter.Get<string>("Source"), Parameter.Get<string>("Destination"), "Populate Search Criteria");

            // Book Source and Designation tickets
            RunStep(SearchPage.BookTicket, "Book ticket");

            // Validate Source of Jouney Details
            RunStep(() => 
            {
                var onwardDetails = PaymentPage.GetOnWardsDetails();
                onwardDetails.RoutName.Should().Equals(Parameter.Get<string>("Source"));    // Asserting Source Name 
                test.Log(Status.Pass, $"** Source Matched => {Parameter.Get<string>("Source")} **");                
                onwardDetails.Boarding.Should().Equals(Parameter.Get<string>("Boarding"));
                test.Log(Status.Pass, $"** Boarding Matched => {Parameter.Get<string>("Boarding")} **");
                }, "Validate Source Details");

            // Validate Return of Jouney Details
            RunStep(() =>
            {
                var returnDetails = PaymentPage.GetReturnDetails();
                returnDetails.RoutName.Should().Equals(Parameter.Get<string>("Destination"));   // Asserting Destination Name 
                test.Log(Status.Pass, $"** Destination Matched => {Parameter.Get<string>("Destination")} **");
                returnDetails.Boarding.Should().Equals(Parameter.Get<string>("ReturnBoarding"));  // Asserting Boarinng Name
                test.Log(Status.Pass, $"** Boarding Matched => {Parameter.Get<string>("ReturnBoarding")} **");
            }, "Validate Return Details");

            // Validate Payment of Jouney Details
            RunStep(() =>
            {
                var SourceAmount = Convert.ToDouble(Parameter.Get<string>("OnwardsAmount"));
                var DesigAmount = Convert.ToDouble(Parameter.Get<string>("ReturnAmount"));
                var SumFair = SourceAmount + DesigAmount;
                var TotalFiar = PaymentPage.GetTotalFiar();
                TotalFiar.Should().Equals(SumFair);     //Asserting Total fair with sum of the fair
                test.Log(Status.Pass, $"** Total Fair Amount:[{TotalFiar}] Matched with SumFiar:[{SumFair}] **");
            }, "Validate Payment Details on Payment Page");


            // Log display on OUTPUT window
        }

    }
}
