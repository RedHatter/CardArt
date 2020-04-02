using System;
﻿using System.IO;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System.Reflection;

namespace HDT.Plugins.CardArt
{
	public class CardArtPlugin : IPlugin
	{
		private object _originalToolTip;
		private double _originalWidth;
		private DependencyPropertyDescriptor _descriptor;
		private String _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public string Author
		{
			get { return "RedHatter"; }
		}

		public string ButtonText
		{
			get { return null; }
		}

		public string Description
		{
			get { return "A simple plugin displaying actual hearthstone art for the card tooltips."; }
		}

		public MenuItem MenuItem
		{
			get { return null; }
		}

		public string Name
		{
			get { return "Card Art"; }
		}

		public void OnButtonPress() {}

		public void OnLoad()
		{
			var tooltip = Core.OverlayWindow.FindName("ToolTipCard") as ContentControl;
			_originalToolTip = tooltip.Content;
			_originalWidth = tooltip.Width;

			var image = new Image();
			_descriptor = DependencyPropertyDescriptor
				.FromProperty(Image.DataContextProperty, typeof(Image));
			_descriptor.AddValueChanged(image, ContextChanged);
			tooltip.Content = image;
			tooltip.Width = 320.0;
		}

		internal void ContextChanged(object sender, EventArgs e) {
			var image = sender as Image;
			var card = image.DataContext as Card;
			image.Source = new BitmapImage(new Uri($"{_basePath}\\assets\\{card.Id}.png"));
		}

		public void OnUnload()
		{
			var tooltip = Core.OverlayWindow.FindName("ToolTipCard") as ContentControl;
			_descriptor.RemoveValueChanged(tooltip.Content, ContextChanged);
			tooltip.Content = _originalToolTip;
			tooltip.Width = _originalWidth;
		}

		public void OnUpdate() {}

		public Version Version
		{
			get { return new Version(1, 0, 0); }
		}
	}
}
