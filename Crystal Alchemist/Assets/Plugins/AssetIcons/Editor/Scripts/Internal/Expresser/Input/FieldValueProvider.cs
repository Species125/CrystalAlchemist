#if UNITY_EDITOR
#pragma warning disable

using System.Reflection;

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	/// <summary>
	/// <para></para>
	/// </summary>
	internal class FieldValueProvider : IValueProvider, IObjectContext
	{
		private readonly FieldInfo targetField;

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns>
		/// <para></para>
		/// </returns>
		public MathValue Value
		{
			get
			{
				return new MathValue(targetField.GetValue(Target));
			}
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public object Target { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetField"></param>
		public FieldValueProvider(FieldInfo targetField)
		{
			this.targetField = targetField;
			Target = null;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetField"></param>
		/// <param name="targetObject"></param>
		public FieldValueProvider(FieldInfo targetField, object targetObject)
		{
			this.targetField = targetField;
			Target = targetObject;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return targetField.Name;
		}
	}
}

#pragma warning restore
#endif
