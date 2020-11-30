using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

namespace Unity.RemoteConfig.Editor.Tests
{
    internal class RemoteConfigDataStoreTests
    {
        [TearDown]
        public void TearDown()
        {
            var path = typeof(RemoteConfigUtilities)
                .GetField("k_PathToDataStore", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null) as string;
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
        }

        [Test]
        public void InitDataStore_InitsAll()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var initDataStoreMethod =
                typeof(RemoteConfigDataStore).GetMethod("InitDataStore", BindingFlags.NonPublic |
                                                                         BindingFlags.Instance);
            initDataStoreMethod?.Invoke(dataStore, new object[] { });

            Assert.That(dataStore.rsKeyList != null);
            Assert.That(dataStore._rsKeyList != null);
            Assert.That(dataStore._rsLastCachedKeyList != null);
            Assert.That(dataStore._rulesList != null);
            Assert.That(dataStore._lastCachedRulesList != null);
            Assert.That(dataStore._environments != null);
        }

        [Test]
        public void CheckAndCreateAssetFolder_CreatesAssetFolder()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var path = typeof(RemoteConfigUtilities)
                .GetField("k_PathToDataStore", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(dataStore) as string;
            Directory.Delete(path, true);
            Assert.That(!Directory.Exists(path));
        }

        [Test]
        public void RSList_ReturnsCorrectKeysTypesAndValues()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);
            foreach(var settingWithMetadata in dataStore.rsKeyList)
            {
                Assert.That(!string.IsNullOrEmpty(settingWithMetadata["metadata"]["entityId"].Value<string>()));
                switch (settingWithMetadata["rs"]["type"].Value<string>())
                {
                    case "bool":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingBool["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingBool["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingBool["value"].Value<string>()));
                        break;
                    case "int":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingInt["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingInt["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingInt["value"].Value<string>()));
                        break;
                    case "float":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingFloat["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingFloat["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingFloat["value"].Value<string>()));
                        break;
                    case "long":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingLong["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingLong["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingLong["value"].Value<string>()));
                        break;
                    case "string":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingString["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingString["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingString["value"].Value<string>()));
                        break;
                    case "json":
                        Assert.That(settingWithMetadata["rs"]["key"].Value<string>().Equals(RCTestUtils.settingJson["key"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["type"].Value<string>().Equals(RCTestUtils.settingJson["type"].Value<string>()));
                        Assert.That(settingWithMetadata["rs"]["value"].Value<string>().Equals(RCTestUtils.settingJson["value"].Value<string>()));
                        break;
                }
            }
        }

        [Test]
        public void SetDefaultEnvironment_ShouldSetDefaultEnvironment()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var newDefaultEnvironment = RCTestUtils.environment2;
            dataStore.environments = RCTestUtils.environments;
            dataStore.SetDefaultEnvironment(newDefaultEnvironment["id"].Value<string>());
            Assert.That(Equals(dataStore.environments[1]["isDefault"].Value<bool>(), true));
        }

        [Test]
        public void SetRulesDataStore_SetsEntityIdOnKeys()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesList;
            var rulesList = dataStore.rulesList;
            foreach(var rule in rulesList)
            {
                foreach(var setting in rule["value"])
                {
                    Assert.That(!string.IsNullOrEmpty(setting["metadata"]["entityId"].Value<string>()));
                    switch (setting["rs"]["type"].Value<string>())
                    {
                        case "bool":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingBool["key"].Value<string>()));
                            break;
                        case "int":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingInt["key"].Value<string>()));
                            break;
                        case "float":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingFloat["key"].Value<string>()));
                            break;
                        case "long":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingLong["key"].Value<string>()));
                            break;
                        case "string":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingString["key"].Value<string>()));
                            break;
                        case "json":
                            Assert.That(setting["rs"]["key"].Value<string>().Equals(RCTestUtils.settingJson["key"].Value<string>()));
                            break;
                    }
                }
            }
        }

        [Test]
        public void GetRuleAtIndex_ReturnsRuleAtIndex()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesList;
            for (int i = 0; i < dataStore.rulesList.Count; i++)
            {
                Assert.That(Equals(dataStore.GetRuleAtIndex(i), dataStore.rulesList[i]));
            }
        }

        [Test]
        public void GetRuleByID_ReturnsRuleWithGivenID()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesList;
            for (int i = 0; i < dataStore.rulesList.Count; i++)
            {
                Assert.That(Equals(dataStore.GetRuleByID(dataStore.rulesList[i]["id"].Value<string>()), dataStore.rulesList[i]));
            }
        }

        [Test]
        public void SetCurrentEnvironment_SetsCurrentEnvironment()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var currentEnvironment = new JObject();
            currentEnvironment["name"] = RCTestUtils.currentEnvironment;
            currentEnvironment["id"] = RCTestUtils.currentEnvironmentId;
            dataStore.SetCurrentEnvironment(currentEnvironment);

            Assert.That(Equals(dataStore.currentEnvironmentName, currentEnvironment["name"].Value<string>()));
            Assert.That(Equals(dataStore.currentEnvironmentId, currentEnvironment["id"].Value<string>()));
        }

        [Test]
        public void SetDataStoreConfig_SetsDataStoreConfigWhenAListIsPassedIn()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var config = new JObject();
            config["type"] = "settings";
            config["id"] = "someId";
            config["value"] = RCTestUtils.rsListNoMetadata;
            dataStore.config = config;
            Assert.That(RCTestUtils.rsListNoMetadata.Count.Equals(dataStore.rsKeyList.Count));
            for(int i = 0; i < RCTestUtils.rsListNoMetadata.Count; i++)
            {
                Assert.That(!string.IsNullOrEmpty(dataStore.rsKeyList[i]["metadata"]["entityId"].Value<string>()));
                Assert.That(dataStore.rsKeyList[i]["rs"]["key"].Equals(RCTestUtils.rsListNoMetadata[i]["key"]));
                Assert.That(dataStore.rsKeyList[i]["rs"]["value"].Equals(RCTestUtils.rsListNoMetadata[i]["value"]));
            }
        }

        [Test]
        public void SetRulesDataStore_SetsRulesStoreWhenAListIsPassedInWithoutSettings()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            for(int i = 0; i < RCTestUtils.rulesWithNoSettingsList.Count; i++)
            {
                Assert.That(dataStore.rulesList[i]["id"].Value<string>().Equals(RCTestUtils.rulesWithNoSettingsList[i]["id"].Value<string>()));
                Assert.That(dataStore.rulesList[i]["name"].Value<string>().Equals(RCTestUtils.rulesWithNoSettingsList[i]["name"].Value<string>()));
            }
        }

        [Test]
        public void SetRulesDataStore_SetsRulesStoreWhenAListIsPassedInWithSettings()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;
            for(int i = 0; i < RCTestUtils.rulesWithSettingsList.Count; i++)
            {
                Assert.That(dataStore.rulesList[i]["id"].Value<string>().Equals(RCTestUtils.rulesWithSettingsList[i]["id"].Value<string>()));
                Assert.That(dataStore.rulesList[i]["name"].Value<string>().Equals(RCTestUtils.rulesWithSettingsList[i]["name"].Value<string>()));
            }
        }

        [Test]
        public void SetRulesDataStore_SetsRulesStoreWhenAListIsPassedInWithAndWithoutSettings()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesList;
            for(int i = 0; i < RCTestUtils.rulesList.Count; i++)
            {
                Assert.That(dataStore.rulesList[i]["id"].Value<string>().Equals(RCTestUtils.rulesList[i]["id"].Value<string>()));
                Assert.That(dataStore.rulesList[i]["name"].Value<string>().Equals(RCTestUtils.rulesList[i]["name"].Value<string>()));
            }
        }

        [Test]
        public void SetRulesDataStore_SetsRulesStoreWhenAListIsPassedInWithFormattedMetadata()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsMetadata;
            for(int i = 0; i < RCTestUtils.rulesWithSettingsMetadata.Count; i++)
            {
                Assert.That(dataStore.rulesList[i]["id"].Value<string>().Equals(RCTestUtils.rulesWithSettingsMetadata[i]["id"].Value<string>()));
                Assert.That(dataStore.rulesList[i]["name"].Value<string>().Equals(RCTestUtils.rulesWithSettingsMetadata[i]["name"].Value<string>()));
            }
        }

        [Test]
        public void SetRulesDataStore_SetsRulesStoreWhenAListIsPassedInWithAndWithoutFormattedMetadata()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithAndWithoutSettingsMetadata;
            for(int i = 0; i < RCTestUtils.rulesWithAndWithoutSettingsMetadata.Count; i++)
            {
                Assert.That(dataStore.rulesList[i]["id"].Value<string>().Equals(RCTestUtils.rulesWithAndWithoutSettingsMetadata[i]["id"].Value<string>()));
                Assert.That(dataStore.rulesList[i]["name"].Value<string>().Equals(RCTestUtils.rulesWithAndWithoutSettingsMetadata[i]["name"].Value<string>()));
            }
        }

       [Test]
        public void SetRulesDataStore_SetsCorrectEntityIdOnSettingsInRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var config = new JObject();
            config["type"] = "settings";
            config["id"] = "someId";
            config["value"] = RCTestUtils.rsListNoMetadata;
            dataStore.config = config;
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;

            var rulesFromDataStore = dataStore.rulesList;
            var settingsFromDataStore = dataStore.rsKeyList;

            for(int i = 0; i < rulesFromDataStore.Count; i++)
            {
                var settingsForCurrentRule = rulesFromDataStore[i]["value"];
                for(int j = 0; j < settingsForCurrentRule.Count(); j++)
                {
                    Assert.That(settingsForCurrentRule[j]["rs"]["key"].Equals(settingsFromDataStore[j]["rs"]["key"]));
                    Assert.That(!string.IsNullOrEmpty(settingsForCurrentRule[j]["metadata"]["entityId"].Value<string>()));
                }
            }
        }

        [Test]
        public void DeleteRule_DeletesRuleFromRulesListAndAddsRuleToDeletedRuleIDs()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;
            var deletedRule = RCTestUtils.rulesWithSettingsList[0];

            dataStore.DeleteRule(deletedRule["id"].Value<string>());

            Assert.That(!dataStore.rulesList.Contains(deletedRule));
            Assert.That(dataStore.deletedRulesIDs.Contains(deletedRule["id"].Value<string>()));
            Assert.That(!dataStore.rulesList.Contains(deletedRule));
        }

        [Test]
        public void UpdateRuleAttributes_ShouldUpdateRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            var oldRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];
            var newRule = (JObject)RCTestUtils.rulesWithNoSettingsList[1];
            newRule["id"] = "very-new-id";
            newRule["name"] = "very-new-name";

            dataStore.UpdateRuleAttributes(oldRule["id"].Value<string>(), newRule);
            Assert.That(dataStore.rulesList.Any(r => r["id"].Value<string>() == newRule["id"].Value<string>()));
            Assert.That(!dataStore.rulesList.Any(r => r["id"].Value<string>() == oldRule["id"].Value<string>()));
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == newRule["id"].Value<string>()));
            Assert.That(rulesFromList[0]["name"].Value<string>() == newRule["name"].Value<string>());
            Assert.That(rulesFromList[0]["enabled"].Value<bool>() == newRule["enabled"].Value<bool>());
        }

        [Test]
        public void EnableOrDisableRule_UpdatesEnabledFieldOfRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            var currentRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];
            var currentRuleId = currentRule["id"].Value<string>();
            var currentRuleEnabled = currentRule["enabled"].Value<bool>();

            dataStore.EnableOrDisableRule(currentRuleId, !currentRuleEnabled);
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            Assert.That(rulesFromList[0]["enabled"].Value<bool>() == !currentRuleEnabled);
        }

        [Test]
        public void AddSettingToRule_ShouldAddRightSettingToRightRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);
            var currentRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];
            var currentRuleId = currentRule["id"].Value<string>();

            dataStore.AddSettingToRule(currentRuleId, RCTestUtils.EntityIdString);
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            Assert.That(rulesFromList[0]["value"][0]["rs"]["key"].Value<string>() == RCTestUtils.settingString["key"].Value<string>());
        }

        [Test]
        public void DeleteSettingFromRule_ShouldRemoveRightSettingsFromRightRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;
            var currentRule = (JObject)RCTestUtils.rulesWithSettingsList[0];
            var currentRuleId = currentRule["id"].Value<string>();
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            var currentSettings = rulesFromList[0]["value"];

            dataStore.DeleteSettingFromRule(currentRuleId, currentSettings[0]["metadata"]["entityId"].Value<string>());
            Assert.That(dataStore.updatedRulesIDs[0].Equals(currentRuleId));

            var rulesFromListAfter = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            var currentSettingsAfter = rulesFromListAfter[0]["value"];
            var hasBoolkey = false;
            for (int i = 0; i < currentSettingsAfter.Count(); i++)
            {
                if (currentSettingsAfter[i]["rs"]["key"].Value<string>() == RCTestUtils.settingBool["key"].Value<string>())
                {
                    hasBoolkey = true;
                }
            }
            Assert.That(hasBoolkey.Equals(false));
        }

        [Test]
        public void UpdateSettingForRule_ShouldUpdateSettingOnRightRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;
            var currentRule = (JObject)RCTestUtils.rulesWithSettingsList[0];
            var currentRuleId = currentRule["id"].Value<string>();
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            var currentSettings = rulesFromList[0]["value"];

            var newSetting = currentSettings[0].DeepClone();
            newSetting["rs"]["key"] = "SettingUpdated";
            dataStore.UpdateSettingForRule(currentRuleId, (JObject)newSetting);
            Assert.That(dataStore.updatedRulesIDs[0].Equals(currentRuleId));

            var rulesFromListAfter = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currentRuleId));
            var currentSettingsAfter = rulesFromListAfter[0]["value"];
            var settingAfter = currentSettingsAfter[0];
            Assert.That(Equals(settingAfter["rs"]["key"], newSetting["rs"]["key"]));
        }

        [Test]
        public void RemoveRuleFromAddedRuleIDs_RemovesRuleFromAddedRules()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.addedRulesIDs = new List<string>(RCTestUtils.updatedRuleIDs);
            dataStore.RemoveRuleFromAddedRuleIDs(RCTestUtils.updatedRuleIDs[0]);
            Assert.That(dataStore.addedRulesIDs.Count == 2);
            Assert.That(!dataStore.addedRulesIDs.Contains(RCTestUtils.updatedRuleIDs[0]));
        }

        [Test]
        public void RemoveRuleFromUpdatedRuleIDs_RemovesRuleFromUpdatedRules()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.updatedRulesIDs = new List<string>(RCTestUtils.updatedRuleIDs);
            dataStore.RemoveRuleFromUpdatedRuleIDs(RCTestUtils.updatedRuleIDs[0]);
            Assert.That(dataStore.updatedRulesIDs.Count == 2);
            Assert.That(!dataStore.updatedRulesIDs.Contains(RCTestUtils.updatedRuleIDs[0]));
        }

        [Test]
        public void RemoveRuleFromDeletedRuleIDs_RemovesRuleFromDeletedRules()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.deletedRulesIDs = new List<string>(RCTestUtils.updatedRuleIDs);
            dataStore.RemoveRuleFromDeletedRuleIDs(RCTestUtils.updatedRuleIDs[0]);
            Assert.That(dataStore.deletedRulesIDs.Count == 2);
            Assert.That(!dataStore.deletedRulesIDs.Contains(RCTestUtils.updatedRuleIDs[0]));
        }

        [Test]
        public void ClearUpdatedRulesLists_ClearsAllLists()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.updatedRulesIDs = new List<string>(RCTestUtils.updatedRuleIDs);
            dataStore.addedRulesIDs = new List<string>(RCTestUtils.addedRuleIDs);
            dataStore.deletedRulesIDs = new List<string>(RCTestUtils.deletedRuleIDs);
            dataStore.ClearUpdatedRulesLists();
            Assert.That(dataStore.updatedRulesIDs.Count == 0);
            Assert.That(dataStore.addedRulesIDs.Count == 0);
            Assert.That(dataStore.deletedRulesIDs.Count == 0);
        }

        [Test]
        public void AddSetting_AddsSettingToListAndDict()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            var newSetting = new JObject();
            newSetting["metadata"] = new JObject();
            newSetting["metadata"]["entityId"] = "new-setting-entitity-id";
            newSetting["rs"] = new JObject();
            newSetting["rs"]["key"] = "NewSetting";
            newSetting["rs"]["value"] = "NewValue";
            newSetting["rs"]["type"] = "string";

            dataStore.AddSetting(newSetting);
            var settingsFromList = new JArray(dataStore.rsKeyList.Where(s => s["rs"]["key"].Value<string>() == newSetting["rs"]["key"].Value<string>()));
            var addedSetting = settingsFromList[0];
            Assert.That(Equals(addedSetting["rs"]["key"], newSetting["rs"]["key"]));
            Assert.That(Equals(addedSetting["rs"]["value"], newSetting["rs"]["value"]));
            Assert.That(Equals(addedSetting["rs"]["type"], newSetting["rs"]["type"]));
        }

        [Test]
        public void AddSettingFormattedAsDate_AddsSettingToListAndDict()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            var newSetting = new JObject();
            newSetting["metadata"] = new JObject();
            newSetting["metadata"]["entityId"] = "new-setting-entitity-id";
            newSetting["rs"] = new JObject();
            newSetting["rs"]["key"] = "NewSetting";
            newSetting["rs"]["value"] = "2020-04-03T10:01:00Z";
            newSetting["rs"]["type"] = "string";

            dataStore.AddSetting(newSetting);
            var settingsFromList = new JArray(dataStore.rsKeyList.Where(s => s["rs"]["key"].Value<string>() == newSetting["rs"]["key"].Value<string>()));
            var addedSetting = settingsFromList[0];
            Assert.That(Equals(addedSetting["rs"]["key"], newSetting["rs"]["key"]));
            Assert.That(Equals(addedSetting["rs"]["value"], newSetting["rs"]["value"]));
            Assert.That(Equals(addedSetting["rs"]["type"], newSetting["rs"]["type"]));
        }

        [Test]
        public void AddSettingStringFormattedAsJson_AddsSettingToListAndDict()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            var newSetting = new JObject();
            newSetting["metadata"] = new JObject();
            newSetting["metadata"]["entityId"] = "new-setting-entitity-id";
            newSetting["rs"] = new JObject();
            newSetting["rs"]["key"] = "NewSetting";
            newSetting["rs"]["value"] = "{\"a\":\"b\"}";
            newSetting["rs"]["type"] = "string";

            dataStore.AddSetting(newSetting);
            var settingsFromList = new JArray(dataStore.rsKeyList.Where(s => s["rs"]["key"].Value<string>() == newSetting["rs"]["key"].Value<string>()));
            var addedSetting = settingsFromList[0];
            Assert.That(Equals(addedSetting["rs"]["key"], newSetting["rs"]["key"]));
            Assert.That(Equals(addedSetting["rs"]["value"], newSetting["rs"]["value"]));
            Assert.That(Equals(addedSetting["rs"]["type"], newSetting["rs"]["type"]));
        }

        [Test]
        public void AddSetting_AddsJsonSettingToListAndDict()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            var newSetting = new JObject();
            newSetting["metadata"] = new JObject();
            newSetting["metadata"]["entityId"] = "new-setting-entitity-id";
            newSetting["rs"] = new JObject();
            newSetting["rs"]["key"] = "NewSetting";
            newSetting["rs"]["value"] = "{'a':'b'}";
            newSetting["rs"]["type"] = "json";

            dataStore.AddSetting(newSetting);
            var settingsFromList = new JArray(dataStore.rsKeyList.Where(s => s["rs"]["key"].Value<string>() == newSetting["rs"]["key"].Value<string>()));
            var addedSetting = settingsFromList[0];
            var jsonKey = JObject.Parse(addedSetting["rs"]["value"].Value<string>());
            Assert.That(Equals(addedSetting["rs"]["key"], newSetting["rs"]["key"]));
            Assert.That(Equals(addedSetting["rs"]["value"], newSetting["rs"]["value"]));
            Assert.That(Equals(addedSetting["rs"]["type"], newSetting["rs"]["type"]));
            Assert.That(Equals(jsonKey["a"].Value<string>(), "b"));
        }

        [Test]
        public void DeleteSetting_DeletesSettingFromListAndDict()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            dataStore.DeleteSetting(RCTestUtils.rsListWithMetadata[0]["metadata"]["entityId"].Value<string>());
            Assert.That(!dataStore.rsKeyList.Contains(RCTestUtils.rsListWithMetadata[0]));
        }

        [Test]
        public void UpdateSetting_UpdatesCorrectSetting()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rsKeyList = new JArray(RCTestUtils.rsListWithMetadata);

            var newSetting = new JObject();
            newSetting["metadata"] = new JObject();
            newSetting["metadata"]["entityId"] = "new-setting-entitity-id";
            newSetting["rs"] = new JObject();
            newSetting["rs"]["key"] = "NewSetting";
            newSetting["rs"]["value"] = "NewValue";
            newSetting["rs"]["type"] = "string";

            var oldSetting = (JObject)dataStore.rsKeyList[0];

            dataStore.UpdateSetting(oldSetting, newSetting);
            Assert.That(!dataStore.rsKeyList.Contains(oldSetting));
            Assert.That(dataStore.rsKeyList.Contains(newSetting));

            var settingsFromList = new JArray(dataStore.rsKeyList.Where(s => s["rs"]["key"].Value<string>() == newSetting["rs"]["key"].Value<string>()));
            var updatedSetting = settingsFromList[0];
            Assert.That(Equals(updatedSetting["rs"]["key"], newSetting["rs"]["key"]));
            Assert.That(Equals(updatedSetting["rs"]["value"], newSetting["rs"]["value"]));
            Assert.That(Equals(updatedSetting["rs"]["type"], newSetting["rs"]["type"]));
        }

        [Test]
        public void HasRules_CorrectlyReturnsTrueIfThereAreRules()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            Assert.That(dataStore.HasRules());
        }

        [Test]
        public void HasRules_CorrectlyReturnsFalseIfThereAreNoRules()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = new JArray();
            Assert.That(dataStore.HasRules() == false);
        }

        [Test]
        public void IsSettingInRule_ReturnsTrueWhenSettingIsInRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;

            var currentRule = dataStore.rulesList[0];
            var currentRuleId = currentRule["id"].Value<string>();
            var currentSettingEntityId = currentRule["value"][0]["metadata"]["entityId"].Value<string>();
            Assert.That(dataStore.IsSettingInRule(currentRuleId, currentSettingEntityId));
        }

        [Test]
        public void IsSettingInRule_ReturnsFalseWhenSettingIsNotInRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithSettingsList;

            var currentRule = dataStore.rulesList[0];
            var currentRuleId = currentRule["id"].Value<string>();
            Assert.That(!dataStore.IsSettingInRule(currentRuleId, "non-existing-entityId"));
        }

        [Test]
        public void ValidateRule_ShouldReturnTrueForValidRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            Assert.That(dataStore.ValidateRule(RCTestUtils.ruleNoValues1));
        }

        [Test]
        public void ValidateRule_ShouldReturnFalseForInvalidRule()
        {
            var dataStore = RCTestUtils.GetDataStore();
            var rule = (JObject)RCTestUtils.ruleNoValues1.DeepClone();
            rule["priority"] = 1001;
            Assert.That(dataStore.ValidateRule(rule) == false);
            rule["priority"] = -1;
            Assert.That(dataStore.ValidateRule(rule) == false);
        }

        [Test]
        public void ConfigID_ShouldReturnConfigIDFromDataStore()
        {
            var dataStore = RCTestUtils.GetDataStore();
            Assert.That(string.IsNullOrEmpty(dataStore.configId));
            dataStore.configId = "someId";
            Assert.That(string.Equals(dataStore.configId, "someId"));
        }

        [Test]
        public void ValidateRule_ShouldReturnFalseForAddingDuplicateRuleName()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = new JArray();
            var rule1 = (JObject)RCTestUtils.ruleNoValues1.DeepClone();
            dataStore.AddRule(rule1);
            var rule2 = (JObject)RCTestUtils.ruleNoValues2.DeepClone();
            dataStore.AddRule(rule2);
            rule2["name"] = rule1["name"].Value<string>();
            Assert.That(dataStore.ValidateRule(rule2) == false);
        }

        [Test]
        public void UpdateRuleType_ShouldReturnRuleOfNewTypeVariant()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            var currRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];

            var newType = "variant";
            dataStore.UpdateRuleType(currRule["id"].Value<string>(), newType);

            Assert.That(dataStore.rulesList.Any(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            Assert.That(rulesFromList[0]["name"].Value<string>() == currRule["name"].Value<string>());
            Assert.That(rulesFromList[0]["type"].Value<string>() == newType);
        }

        [Test]
        public void UpdateRuleTypeForSameType_ShouldReturnSameRuleTypeSegmentation()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            var currRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];

            var newType = "segmentation";
            dataStore.UpdateRuleType(currRule["id"].Value<string>(), newType);

            Assert.That(dataStore.rulesList.Any(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            Assert.That(rulesFromList[0]["name"].Value<string>() == currRule["name"].Value<string>());
            Assert.That(rulesFromList[0]["type"].Value<string>() == newType);
        }

        [Test]
        public void UpdateRuleTypeForSameType_ShouldReturnSameRuleTypeVariant()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;
            var currRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];

            // segmentation to variant
            var newType = "variant";
            dataStore.UpdateRuleType(currRule["id"].Value<string>(), newType);

            Assert.That(dataStore.rulesList.Any(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            Assert.That(rulesFromList[0]["name"].Value<string>() == currRule["name"].Value<string>());
            Assert.That(rulesFromList[0]["type"].Value<string>() == newType);

            // variant to variant
            newType = "variant";
            dataStore.UpdateRuleType(currRule["id"].Value<string>(), newType);

            Assert.That(dataStore.rulesList.Any(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            Assert.That(rulesFromList[0]["name"].Value<string>() == currRule["name"].Value<string>());
            Assert.That(rulesFromList[0]["type"].Value<string>() == newType);
        }

        [Test]
        public void AddingRuleOfVariantTypeInDataStore_ShouldAddVariantTypeToRulesList()
        {
            var dataStore = RCTestUtils.GetDataStore();
            dataStore.rulesList = RCTestUtils.rulesWithNoSettingsList;

            var mixedTypeRulesList = new JArray();
            var currRule = (JObject)RCTestUtils.rulesWithNoSettingsList[0];
            var newType = "variant";
            dataStore.UpdateRuleType(currRule["id"].Value<string>(), newType);

            var rulesFromList = new JArray(dataStore.rulesList.Where(r => r["id"].Value<string>() == currRule["id"].Value<string>()));
            var variantRule = rulesFromList[0];

            mixedTypeRulesList.Add((JObject)RCTestUtils.rulesWithNoSettingsList[1]); // type segmentation
            mixedTypeRulesList.Add(variantRule); // type variant

            dataStore.rulesList = mixedTypeRulesList;

            Assert.That(mixedTypeRulesList.Count == dataStore.rulesList.Count);
            Assert.That(dataStore.rulesList[0]["type"].Value<string>() == "segmentation");
            Assert.That(dataStore.rulesList[1]["type"].Value<string>() == newType);
        }
    }

    internal static class RCTestUtils
    {
        public const string EntityIdBool = "bool0000-0000-0000-0000-000000000000";
        public const string EntityIdInt = "int00000-0000-0000-0000-000000000000";
        public const string EntityIdLong = "long0000-0000-0000-0000-000000000000";
        public const string EntityIdFloat = "float000-0000-0000-0000-000000000000";
        public const string EntityIdString = "string00-0000-0000-0000-000000000000";
        public const string EntityIdJson = "json0000-0000-0000-0000-000000000000";
        public const int IntValue = 1;
        public const float FloatValue = 1.0f;
        public const bool BoolValue = true;
        public const long LongValue = 32L;
        public const string StringValue = "test-string";
        public const string JsonValue = "{'keyA': [{ 'subkeyB': 'subValueB','subKeyC': {'subSubKeyD': 0}}]}";

        public const string currentEnvironment = "test-environment";
        public const string currentEnvironmentId = "test-environment-id";

        public static JObject settingBool = new JObject
        {
            {"key", "SettingBool"},
            {"type", "bool"},
            {"value", BoolValue}
        };

        public static JObject settingInt = new JObject
        {
            {"key", "SettingInt"},
            {"type", "int"},
            {"value", IntValue}
        };

        public static JObject settingLong = new JObject
        {
            {"key", "settingLong"},
            {"type", "long"},
            {"value", LongValue}
        };

        public static JObject settingFloat = new JObject
        {
            {"key", "SettingFloat"},
            {"type", "float"},
            {"value", FloatValue}
        };

        public static JObject settingString = new JObject
        {
            {"key", "SettingString"},
            {"type", "string"},
            {"value", StringValue}
        };

        public static JObject settingJson = new JObject
        {
            {"key", "settingJson"},
            {"type", "json"},
            {"value", JsonValue}
        };

        public static JObject settingBoolWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdBool}}},
            {"rs", settingBool}
        };

        public static JObject settingIntWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdInt}}},
            {"rs", settingInt}
        };

        public static JObject settingLongWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdLong}}},
            {"rs", settingLong}
        };

        public static JObject settingFloatWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdFloat}}},
            {"rs", settingFloat}
        };

        public static JObject settingStringWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdString}}},
            {"rs", settingString}
        };

        public static JObject settingJsonWithMetadata = new JObject
        {
            {"metadata", new JObject {{"entityId", EntityIdJson}}},
            {"rs", settingJson}
        };


        // settings
        public static JArray rsListWithMetadata = new JArray
        {
            settingBoolWithMetadata,
            settingIntWithMetadata,
            settingLongWithMetadata,
            settingFloatWithMetadata,
            settingStringWithMetadata,
            settingJsonWithMetadata
        };

        public static JArray rsListNoMetadata = new JArray {
            settingBool,
            settingInt,
            settingLong,
            settingFloat,
            settingString,
            settingJson
        };

        // rules
        public static JObject rule1 = new JObject
        {
            {"id", "rule-id-1"},
            {"name", "rule-name-1"},
            {"enabled", true},
            {"priority", 1000},
            {"condition", "true"},
            {"rolloutPercentage", 100},
            {"startDate", "2019-07-10T23:15:14.000-0700"},
            {"endDate", "2019-08-12T08:15:14.000+0430"},
            {"value", new JObject { }}
        };

        public static JObject rule2 = new JObject
        {
            {"id", "rule-id-2"},
            {"name", "rule-name-2"},
            {"enabled", true},
            {"priority", 1000},
            {"condition", "true"},
            {"rolloutPercentage", 100},
            {"startDate", "2019-07-10T23:15:14.000-0700"},
            {"endDate", "2019-08-12T08:15:14.000+0430"},
            {"value", new JObject { }}
        };

        public static JObject rule3 = new JObject
        {
            {"id", "rule-id-3"},
            {"name", "rule-name-3"},
            {"enabled", true},
            {"priority", 1000},
            {"condition", "true"},
            {"rolloutPercentage", 100},
            {"startDate", "2019-07-10T23:15:14.000-0700"},
            {"endDate", "2019-08-12T08:15:14.000+0430"},
            {"value", new JObject { }}
        };

        public static JObject rule4 = new JObject
        {
            {"id", "rule-id-4"},
            {"name", "rule-name-4"},
            {"enabled", true},
            {"priority", 1000},
            {"condition", "true"},
            {"rolloutPercentage", 100},
            {"startDate", "2019-07-10T23:15:14.000-0700"},
            {"endDate", "2019-08-12T08:15:14.000+0430"},
            {"value", new JObject { }}
        };

        public static JObject ruleNoValues1 = AddTypeAndSettingsToRule(JObject.FromObject(rule1), "segmentation", null);
        public static JObject ruleNoValues2 = AddTypeAndSettingsToRule(JObject.FromObject(rule2), "segmentation", null);
        public static JObject ruleValuesWithMetadata1 = AddTypeAndSettingsToRule(JObject.FromObject(rule3), "segmentation", rsListWithMetadata);
        public static JObject ruleValuesNoMetadata1 = AddTypeAndSettingsToRule(JObject.FromObject(rule4), "segmentation", rsListNoMetadata);

        public static Dictionary<string, JObject> rulesDict = new Dictionary<string, JObject>()
        {
            {
                ruleNoValues1["name"].Value<string>(), ruleNoValues1
            },
            {
                ruleNoValues2["name"].Value<string>(), ruleNoValues2
            },
            {
                ruleValuesNoMetadata1["name"].Value<string>(), ruleValuesNoMetadata1
            }
        };

        public static Dictionary<string, JObject> rulesDictWithSettings = new Dictionary<string, JObject>()
        {
            {
                ruleValuesNoMetadata1["name"].Value<string>(), ruleValuesNoMetadata1
            }
        };

        public static Dictionary<string, JObject> rulesDictWithNoSettings = new Dictionary<string, JObject>()
        {
            {
                ruleNoValues1["name"].Value<string>(), ruleNoValues1
            },
            {
                ruleNoValues2["name"].Value<string>(), ruleNoValues2
            }
        };

        public static Dictionary<string, JObject> rulesDictWithSettingsMetadata = new Dictionary<string, JObject>()
        {
            {
                ruleValuesWithMetadata1["name"].Value<string>(), ruleValuesWithMetadata1
            }
        };

        public static Dictionary<string, JObject> rulesDictWithAndWithoutSettingsMetadata = new Dictionary<string, JObject>()
        {
            {
                ruleNoValues1["name"].Value<string>(), ruleNoValues1
            },
            {
                ruleValuesWithMetadata1["name"].Value<string>(), ruleValuesWithMetadata1
            }
        };

        public static JObject AddTypeAndSettingsToRule(JObject jRule, string type, JArray values)
        {
            var newJRule = new JObject();
            newJRule["id"] = jRule["id"].Value<string>();
            newJRule["name"] = jRule["name"].Value<string>();
            newJRule["enabled"] = jRule["enabled"].Value<bool>();
            newJRule["priority"] = jRule["priority"].Value<int>();
            newJRule["condition"] = jRule["condition"].Value<string>();
            newJRule["rolloutPercentage"] = jRule["rolloutPercentage"].Value<int>();
            newJRule["startDate"] = jRule["startDate"].Value<string>();
            newJRule["endDate"] = jRule["endDate"].Value<string>();
            newJRule["type"] = type;
            newJRule["value"] = values;
            return newJRule;
        }

        public static JArray rulesList = new JArray(rulesDict.Values.ToList());
        public static JArray rulesWithSettingsList = new JArray(rulesDictWithSettings.Values.ToList());
        public static JArray rulesWithNoSettingsList = new JArray(rulesDictWithNoSettings.Values.ToList());
        public static JArray rulesWithSettingsMetadata = new JArray(rulesDictWithSettingsMetadata.Values.ToList());
        public static JArray rulesWithAndWithoutSettingsMetadata = new JArray(rulesDictWithAndWithoutSettingsMetadata.Values.ToList());

        public static List<string> addedRuleIDs = new List<string>()
        {
            "added-rule-id-1",
            "added-rule-id-2",
            "added-rule-id-3"
        };

        public static List<string> updatedRuleIDs = new List<string>()
        {
            "updated-rule-id-1",
            "updated-rule-id-2",
            "updated-rule-id-3"
        };

        public static List<string> deletedRuleIDs = new List<string>()
        {
            "deleted-rule-id-1",
            "deleted-rule-id-2",
            "deleted-rule-id-3"
        };

        // environments
        public static JObject environment1 = new JObject
        {
            {"id", "env-id-1"},
            {"project_id", "app-id-1"},
            {"name", "env-name-1"},
            {"description", "env-description-1"},
            {"created-at", "2019-07-10T23:15:14.000-0700"},
            {"updated-at", "2019-08-12T08:15:14.000+0430"},
            {"isDefault", true}
        };

        public static JObject environment2 = new JObject
        {
            {"id", "env-id-2"},
            {"project_id", "app-id-2"},
            {"name", "env-name-2"},
            {"description", "env-description-1"},
            {"created-at", "2019-07-10T23:15:14.000-0700"},
            {"updated-at", "2019-08-12T08:15:14.000+0430"},
            {"isDefault", false}
        };

        public static JArray environments = new JArray
        {
            environment1,
            environment2
        };

        public static RemoteConfigDataStore GetDataStore()
        {
            var dataStore = RemoteConfigUtilities.CheckAndCreateDataStore();
            dataStore.rsKeyList = new JArray();
            dataStore.rulesList = new JArray();
            dataStore.environments = new JArray();
            dataStore.rsLastCachedKeyList = new JArray();
            dataStore.lastCachedRulesList = new JArray();
            dataStore.currentEnvironmentName = "Please create an environment.";
            dataStore.currentEnvironmentId = null;
            dataStore.addedRulesIDs = new List<string>();
            dataStore.updatedRulesIDs = new List<string>();
            dataStore.deletedRulesIDs = new List<string>();
            return dataStore;
        }
    }
}