using NUnit.Framework;
using System;
using Newtonsoft.Json.Linq;

namespace Unity.RemoteConfig.Editor.Tests
{
    internal class WebUtilityTests
    {

        [Test]
        public void FetchEnvironments_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchEnvironments(null);
            }
            catch(ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void FetchDefaultEnvironment_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchDefaultEnvironment(null);
            }
            catch(ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void FetchConfigs_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchConfigs(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void FetchRules_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchRules(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PutConfig_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PutConfig(null, null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PostConfig_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PostConfig(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

      [Test]
        public void PostAddRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PostAddRule(null, null, null, new JObject());
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void PutEditRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.PutEditRule(null, null, null, new JObject());
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void DeleteRule_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.DeleteRule(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void CreateEnvironment_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.CreateEnvironment(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void UpdateEnvironment_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.UpdateEnvironment(null, null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void DeleteEnvironment_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.DeleteEnvironment(null, null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void SetDefaultEnvironment_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.FetchDefaultEnvironment(null,null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }

        [Test]
        public void DeleteConfig_ThrowsArgumentExceptionOnBadArgs()
        {
            try
            {
                RemoteConfigWebApiClient.DeleteConfig(null,null);
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.GetType() == typeof(ArgumentException));
            }
        }
    }
}

