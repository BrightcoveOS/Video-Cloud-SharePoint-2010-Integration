using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace BrightcoveSDK
{
	public class BrightcoveConfig : ConfigurationSection
	{
		[ConfigurationProperty("accounts", IsDefaultCollection = false)]
		public AccountCollection Accounts {
			get {
				AccountCollection accountCollection = (AccountCollection)base["accounts"];
				return accountCollection;
			}
		}

		protected override void DeserializeSection(System.Xml.XmlReader reader) {
			base.DeserializeSection(reader);
		}

		protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode) {
			string s = base.SerializeSection(parentElement, name, saveMode);
			return s;
		}
	}

	public class AccountCollection : ConfigurationElementCollection
	{

		public AccountCollection() {
			//AccountConfigElement account = (AccountConfigElement)CreateNewElement();
			//Add(account);
		}

		protected override ConfigurationElement CreateNewElement() {
			return new AccountConfigElement();
		}

		protected override ConfigurationElement CreateNewElement(string elementName) {
			return new AccountConfigElement(elementName);
		}

		protected override Object GetElementKey(ConfigurationElement element) {
			return ((AccountConfigElement)element).Name;
		}

		public AccountConfigElement this[int index] {
			get {
				return (AccountConfigElement)BaseGet(index);
			}
			set {
				if (BaseGet(index) != null) {
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		new public AccountConfigElement this[string Name] {
			get {
				return (AccountConfigElement)BaseGet(Name);
			}
		}

		public int IndexOf(AccountConfigElement account) {
			return BaseIndexOf(account);
		}

		public void Add(AccountConfigElement account) {
			BaseAdd(account);
		}

		public void Remove(AccountConfigElement account) {
			if (BaseIndexOf(account) >= 0)
				BaseRemove(account.Name);
		}

		////////////////////////////////////////////////////

		#region override simple base methods

		public override ConfigurationElementCollectionType CollectionType {
			get {
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		public new string AddElementName {
			get { return base.AddElementName; }
			set { base.AddElementName = value; }
		}

		public new string ClearElementName {
			get { return base.ClearElementName; }
			set { base.AddElementName = value; }
		}

		public new string RemoveElementName {
			get { return base.RemoveElementName; }
		}

		public new int Count {
			get { return base.Count; }
		}

		protected override void BaseAdd(ConfigurationElement element) {
			BaseAdd(element, false);
		}

		public void RemoveAt(int index) {
			BaseRemoveAt(index);
		}

		public void Remove(string name) {
			BaseRemove(name);
		}

		public void Clear() {
			BaseClear();
		}

		#endregion override simple base methods
	}

	public class AccountConfigElement : ConfigurationElement
	{

		public AccountConfigElement() {
		}

		public AccountConfigElement(string name) {
			Name = name;
		}

		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name {
			get {
				return (string)this["name"];
			}
			set {
				this["name"] = value;
			}
		}

		[ConfigurationProperty("publisherId", IsRequired = true)]
		public long PublisherID {
			get {
				return (long)this["publisherId"];
			}
			set {
				this["publisherId"] = value;
			}
		}

		[ConfigurationProperty("type", IsRequired = true, DefaultValue=AccountType.Video)]
		public AccountType Type {
			get {
				return (AccountType)this["type"];
			}
			set {
				this["type"] = value;
			}
		}

		[ConfigurationProperty("ReadToken", IsRequired = true)]
		public Token ReadToken {
			get {
				return (Token)this["ReadToken"];
			}
			set {
				this["ReadToken"] = value;
			}
		}

		[ConfigurationProperty("ReadURL", IsRequired = true)]
		public Token ReadURL {
			get {
				return (Token)this["ReadURL"];
			}
			set {
				this["ReadURL"] = value;
			}
		}

		[ConfigurationProperty("WriteToken", IsRequired = true)]
		public Token WriteToken {
			get {
				return (Token)this["WriteToken"];
			}
			set {
				this["WriteToken"] = value;
			}
		}

		[ConfigurationProperty("WriteURL", IsRequired = true)]
		public Token WriteURL {
			get {
				return (Token)this["WriteURL"];
			}
			set {
				this["WriteURL"] = value;
			}
		}

		protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey) {
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey) {
			bool ret = base.SerializeElement(writer, serializeCollectionKey);
			return ret;
		}

		protected override bool IsModified() {
			bool ret = base.IsModified();
			return ret;
		}
	}

	public class Token : ConfigurationElement
	{

		[ConfigurationProperty("value", IsRequired = true)]
		public string Value {
			get {
				return (string)this["value"];
			}
			set {
				this["value"] = value;
			}
		}

	}
}
