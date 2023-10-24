using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public sealed class AboutForm : Form
{
	private IContainer iContainer = null;

	private TextBox tbFeature;

	internal Label lblCopyRight;

	internal Label lblRegistered;

	internal Label lblProduct;

	internal Label lblData;

	internal Label lblVersion;

	internal PictureBox pbLogo;

	private LinkLabel linkLabelMain;

	internal Label lblExpired;

	public AboutForm()
	{
		InitUI();
	}

	private void OnFormLoad(object objArg0, EventArgs arg1)
	{
        lblExpired.Text = lblExpired.Text + ApplicationManager.expireDate.ToString("dd.MM.yyyy");
		lblVersion.Text += ApplicationManager.version;
		linkLabelMain.Text = ApplicationManager.strContact;
		if (File.Exists("features.txt"))
		{
			try
			{
                tbFeature.Text = File.ReadAllText("features.txt");
			}
			catch
			{
			}
		}
	}

	private void OnClickLinkMail(object objArg0, LinkLabelLinkClickedEventArgs arg1)
	{
		if (linkLabelMain.Text.IndexOf("@") > 1)
		{
			Process.Start("mailto:" + linkLabelMain.Text);
		}
		else
		{
			Process.Start(linkLabelMain.Text);
		}
	}

	//private void QZJAJSUDZRU2NB8TX4XLDG26CD65B724(object objArg0, EventArgs arg)
	//{
	//}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (iContainer != null)
			{
				iContainer.Dispose();
			}
			base.Dispose(disposing);
		}
		else
		{
			base.Dispose(disposing);
		}
	}

	private void InitUI()
	{
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AboutForm));
		tbFeature = new TextBox();
		lblCopyRight = new Label();
		lblRegistered = new Label();
		lblProduct = new Label();
		lblData = new Label();
		lblVersion = new Label();
		pbLogo = new PictureBox();
		linkLabelMain = new LinkLabel();
		lblExpired = new Label();
		((ISupportInitialize)pbLogo).BeginInit();
		SuspendLayout();

        tbFeature.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 204);
		tbFeature.Location = new Point(12, 78);
		tbFeature.Multiline = true;
        tbFeature.Name = "textBox1";
		tbFeature.ReadOnly = true;
		tbFeature.ScrollBars = ScrollBars.Vertical;
		tbFeature.Size = new Size(458, 113);
		tbFeature.TabIndex = 92;
        tbFeature.Text = "Features:\r\n- 6 000 000 and more filtered numbers\r\n- very fast speed\r\n- save WhatsApp/NonWhatsApp lists\r\n- AutoSave result";

		lblCopyRight.AutoSize = true;
		lblCopyRight.BackColor = Color.Transparent;
		lblCopyRight.Location = new Point(12, 219);
        lblCopyRight.Name = "label_3";
		lblCopyRight.Size = new Size(90, 13);
		lblCopyRight.TabIndex = 90;
        lblCopyRight.Text = "Copyright © 2022";

		lblRegistered.AutoSize = true;
		lblRegistered.BackColor = Color.Transparent;
		lblRegistered.Location = new Point(12, 194);
        lblRegistered.Name = "label_4";
		lblRegistered.Size = new Size(101, 13);
		lblRegistered.TabIndex = 89;
        lblRegistered.Text = "License: Registered";

		lblProduct.AutoSize = true;
		lblProduct.BackColor = Color.Transparent;
		lblProduct.Location = new Point(99, 59);
        lblProduct.Name = "label_5";
		lblProduct.Size = new Size(82, 13);
		lblProduct.TabIndex = 88;
        lblProduct.Text = "WhatsApp Filter";

		lblData.AutoSize = true;
		lblData.BackColor = Color.Transparent;
		lblData.Location = new Point(99, 37);
        lblData.Name = "label_6";
		lblData.Size = new Size(100, 13);
		lblData.TabIndex = 87;
        lblData.Text = "Date: 27 Sep, 2022";

		lblVersion.AutoSize = true;
		lblVersion.BackColor = Color.Transparent;
		lblVersion.Location = new Point(99, 15);
        lblVersion.Name = "label_7";
		lblVersion.Size = new Size(48, 13);
		lblVersion.TabIndex = 86;
        lblVersion.Text = "Version: ";

		pbLogo.BackColor = Color.Transparent;
		pbLogo.BackgroundImageLayout = ImageLayout.None;
		pbLogo.Cursor = Cursors.IBeam;
		pbLogo.Image = AppResourceManager.GetLogoBmp();
		pbLogo.Location = new Point(12, 12);
        pbLogo.Name = "pictureBox_0";
		pbLogo.Size = new Size(60, 60);
		pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
		pbLogo.TabIndex = 85;
		pbLogo.TabStop = false;

		linkLabelMain.BackColor = Color.Transparent;
        linkLabelMain.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 204);
		linkLabelMain.Location = new Point(137, 206);
        linkLabelMain.Name = "linkLabel3";
		linkLabelMain.Size = new Size(333, 26);
		linkLabelMain.TabIndex = 99;
		linkLabelMain.TabStop = true;
        linkLabelMain.Text = "wasoft@pm.me";
		linkLabelMain.TextAlign = ContentAlignment.MiddleRight;
		linkLabelMain.LinkClicked += OnClickLinkMail;

		lblExpired.BackColor = Color.Transparent;
		lblExpired.ForeColor = Color.DarkRed;
		lblExpired.Location = new Point(318, 15);
        lblExpired.Name = "label1";
		lblExpired.Size = new Size(147, 13);
		lblExpired.TabIndex = 101;
        lblExpired.Text = "Expired: ";
		lblExpired.TextAlign = ContentAlignment.MiddleRight;

		base.AutoScaleDimensions = new SizeF(6f, 13f);
		base.AutoScaleMode = AutoScaleMode.Font;
		BackgroundImage = AppResourceManager.GetWallpaperBitmap();
		base.ClientSize = new Size(477, 246);
		base.Controls.Add(lblExpired);
		base.Controls.Add(linkLabelMain);
		base.Controls.Add(tbFeature);
		base.Controls.Add(lblCopyRight);
		base.Controls.Add(lblRegistered);
		base.Controls.Add(lblProduct);
		base.Controls.Add(lblData);
		base.Controls.Add(lblVersion);
		base.Controls.Add(pbLogo);
		base.FormBorderStyle = FormBorderStyle.FixedSingle;
		string name10 = "$this.Icon";
		base.Icon = (Icon)componentResourceManager.GetObject(name10);
		base.MaximizeBox = false;
		string name11 = "Info";
		base.Name = name11;
		base.ShowInTaskbar = false;
		base.StartPosition = FormStartPosition.CenterScreen;
		string text9 = "Info";
		Text = text9;
		base.Load += OnFormLoad;
		((ISupportInitialize)pbLogo).EndInit();
		ResumeLayout(performLayout: false);
		PerformLayout();
	}
}
