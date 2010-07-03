﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using XTile;
using XTile.Dimensions;
using XTile.Layers;

using TileMapEditor.Commands;

namespace TileMapEditor.Dialogs
{
    public partial class LayerPropertiesDialog : Form
    {
        #region Private Variables

        private Layer m_layer;
        private bool m_isNewLayer;

        #endregion

        #region Private Methods

        private void MarkAsModified()
        {
            m_buttonApply.Enabled = m_buttonOk.Enabled = true;
            m_buttonCancel.Text = "&Cancel";
            m_buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private void OnFieldChanged(object sender, EventArgs e)
        {
            MarkAsModified();
        }

        private void OnPropertyChangedOrDeleted(object sender,
            TileMapEditor.Controls.CustomPropertyEventArgs customPropertyEventArgs)
        {
            MarkAsModified();
        }

        private void OnDialogLoad(object sender, EventArgs eventArgs)
        {
            m_textBoxId.Text = m_layer.Id;
            m_textBoxDescription.Text = m_layer.Description;

            m_numericLayerWidth.Value = m_layer.LayerSize.Width;
            m_numericLayerHeight.Value = m_layer.LayerSize.Height;
            m_numericTileWidth.Value = m_layer.TileSize.Width;
            m_numericTileHeight.Value = m_layer.TileSize.Height;

            m_checkBoxVisible.Checked = m_layer.Visible;

            m_customPropertyGrid.LoadProperties(m_layer);

            if (!m_isNewLayer)
            {
                m_buttonApply.Enabled = m_buttonOk.Enabled = false;
                m_buttonCancel.Text = "&Close";
                m_buttonCancel.DialogResult = DialogResult.OK;
            }
        }

        private void OnDialogOk(object sender, EventArgs eventArgs)
        {
            OnDialogApply(sender, eventArgs);
        }

        private void OnDialogApply(object sender, EventArgs e)
        {
            string newId = m_textBoxId.Text;

            foreach (Layer layer in m_layer.Map.Layers)
            {
                if (layer == m_layer)
                    continue;
                if (newId == layer.Id)
                {
                    MessageBox.Show(this, "The specified Id is already used by another Layer",
                        "Layer Properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Size newLayerSize = new Size((int)m_numericLayerWidth.Value, (int)m_numericLayerHeight.Value);
            Size newTileSize = new Size((int)m_numericTileWidth.Value, (int)m_numericTileHeight.Value);

            Command command = null;

            if (m_isNewLayer)
            {
                m_layer.Id = newId;
                m_layer.Description = m_textBoxDescription.Text;
                m_layer.LayerSize = newLayerSize;
                m_layer.TileSize = newTileSize;
                command = new LayerNewCommand(m_layer.Map, m_layer);

                m_isNewLayer = false;
            }
            else
                command = new LayerPropertiesCommand(m_layer, newId, m_textBoxDescription.Text,
                    newLayerSize, newTileSize, m_checkBoxVisible.Checked,
                    m_customPropertyGrid.NewProperties);

            CommandHistory.Instance.Do(command);

            m_buttonApply.Enabled = m_buttonOk.Enabled = false;
            m_buttonCancel.Text = "&Close";
            m_buttonCancel.DialogResult = DialogResult.OK;
        }

        #endregion

        #region Public Methods

        public LayerPropertiesDialog(Layer layer, bool isNewLayer)
        {
            InitializeComponent();

            m_layer = layer;
            m_isNewLayer = isNewLayer;
        }

        #endregion
    }
}