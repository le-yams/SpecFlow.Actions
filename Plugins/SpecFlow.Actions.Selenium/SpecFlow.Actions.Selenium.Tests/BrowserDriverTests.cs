﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

namespace SpecFlow.Actions.Selenium.Tests
{
    public class BrowserDriverTests
    {
        class MockSeleniumConfiguration : ISeleniumConfiguration
        {
            public Browser Browser { get; set; }

            public string[]? Arguments { get; set; }

            public Dictionary<string, object>? Capabilities { get; set; }
        }

        class MockDriverInitialiser : IDriverInitialiser
        {
            public IWebDriver GetChromeDriver(Dictionary<string, object>? capabilities = null, string[]? args = null)
            {
                throw new NotImplementedException();
            }

            public IWebDriver GetEdgeDriver(Dictionary<string, object>? capabilities)
            {
                throw new NotImplementedException();
            }

            public IWebDriver GetFirefoxDriver(Dictionary<string, object>? capabilities = null, string[]? args = null)
            {
                throw new NotImplementedException();
            }

            public IWebDriver GetInternetExplorerDriver(Dictionary<string, object>? capabilities)
            {
                throw new NotImplementedException();
            }
        }

        public class BrowserDriverAccessor : BrowserDriver
        {
            public Lazy<IWebDriver> CurrentWebDriverLazy => _currentWebDriverLazy;

            public BrowserDriverAccessor(ISeleniumConfiguration seleniumConfiguration, IDriverInitialiser driverInitialiser) : base(seleniumConfiguration, driverInitialiser)
            {
            }
        }

        [Fact]
        public void Current_NotInstantiatedAfterCreation()
        {
            var target = new BrowserDriverAccessor(new MockSeleniumConfiguration(), new MockDriverInitialiser());

            target.CurrentWebDriverLazy.IsValueCreated.Should().BeFalse();
        }

        [Fact]
        public void Current_AfterAccessing_Instantiated()
        {
            var target = new BrowserDriverAccessor(new MockSeleniumConfiguration() { Browser = Browser.Noop }, new MockDriverInitialiser());

            var webdriver = target.Current;

            target.CurrentWebDriverLazy.IsValueCreated.Should().BeTrue();
            webdriver.Should().NotBeNull();
        }
    }
}