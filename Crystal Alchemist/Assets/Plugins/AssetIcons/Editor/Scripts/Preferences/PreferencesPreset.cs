﻿#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>Represents user preferences in a serializable format.</para>
	/// </summary>
	/// <seealso cref="AssetIconsPreferences"/>
	public sealed class PreferencesPreset : ScriptableObject
	{
		[SerializeField]
		[Tooltip("Controls whether AssetIcons should be enabled or disabled.")]
		private BoolEventField enabled = new BoolEventField(true);

		[SerializeField]
		[Tooltip("Enables previewing of Unity GUIStyle assets.")]
		private BoolEventField drawGUIStyles = new BoolEventField(false);

		[SerializeField]
		[FromValues(16, 32, 64, 128, 256)]
		[Tooltip("Allows for adjusting the resolution AssetIcons will render Prefabs with.")]
		private IntEventField prefabResolution = new IntEventField(128);

		[SerializeField]
		[Tooltip("Controls how strong of a tint is applied to AssetIcons rendered graphics when selected.")]
		private ColorTintEventField selectionTint = new ColorTintEventField(new ColorTint(0.4f));

		[SerializeField]
		[Tooltip("A collection of graphics associated with file extensions that AssetIcons uses to render custom file icons")]
		private IconMapping typeIcons = new IconMapping();

		/// <summary>
		/// <para>Controls whether AssetIcons should be enabled or disabled.</para>
		/// </summary>
		public BoolEventField Enabled
		{
			get
			{
				return enabled;
			}
		}

		/// <summary>
		/// <para>Enables previewing of Unity <see cref="GUIStyle"/> assets.</para>
		/// </summary>
		public BoolEventField DrawGUIStyles
		{
			get
			{
				return drawGUIStyles;
			}
		}

		/// <summary>
		/// <para>Allows for adjusting the resolution AssetIcons will render Prefabs with.</para>
		/// </summary>
		/// <remarks>
		/// <para>If you are struggling with performance with a large amount of rendered assets, you could try
		/// adjusting this to boost performance.</para>
		/// </remarks>
		public IntEventField PrefabResolution
		{
			get
			{
				return prefabResolution;
			}
		}

		/// <summary>
		/// <para>Controls how strong of a tint is applied to AssetIcons rendered graphics when selected.</para>
		/// </summary>
		public ColorTintEventField SelectionTint
		{
			get
			{
				return selectionTint;
			}
		}

		/// <summary>
		/// <para>A collection of graphics associated with file extensions that AssetIcons uses to render custom
		/// file icons.</para>
		/// </summary>
		public IconMapping TypeIcons
		{
			get
			{
				return typeIcons;
			}
		}

		private void OnValidate()
		{
			EditorApplication.RepaintProjectWindow();
		}
	}
}

#pragma warning restore
#endif
