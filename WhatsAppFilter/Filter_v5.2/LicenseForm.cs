using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class LicenseForm : Form
{
	private IContainer iContainer = null;

	private Label lblUserName;

	private Label lblSerialKey;

	private Label lblHardwareId;

	private TextBox tbHardwareID;

	private TextBox tbUserName;

	private TextBox tbSerialKey;

	private Button btnActivate;

	private Button btnClose;

	private Button btnCopy;

	private GroupBox groupBox1;

	private PictureBox pictureBox2;

	public LicenseForm()
	{
		InitUI();
	}

	protected static DateTime Tick2DateTime(long nlTick)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(nlTick);
	}

	public static byte[] LicenseStr2Data(string strData)
	{
		strData = strData.Replace("-", "");
		byte[] array = new byte[strData.Length / 2];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = Convert.ToByte(strData.Substring(j * 2, 2), 16);
		}
		return array;
	}

	private void OnClickBtnActivate(object objArg0, EventArgs arg)
	{
		string[] contents;
		if (!(tbUserName.Text.Trim() == ""))
		{
			if (tbSerialKey.Text.Trim() == "")
			{
				Environment.Exit(0);
			}
			contents = new string[2]
			{
				tbUserName.Text.Trim(),
				tbSerialKey.Text.Trim()
			};
		}
		else
		{
			Environment.Exit(0);
			contents = new string[2]
			{
				tbUserName.Text.Trim(),
				tbSerialKey.Text.Trim()
			};
		}
		string path2 = Path.Combine(Application.StartupPath, "license.dat");
		try
		{
			File.WriteAllLines(path2, contents);
		}
		catch (Exception)
		{
		}
		Environment.Exit(0);
	}

	private void OnClickBtnClose(object objArg0, EventArgs arg)
	{
		Process.GetCurrentProcess().Kill();
	}

	private void OnClickBtnCopy(object objArg0, EventArgs arg)
	{
		Clipboard.SetText(tbHardwareID.Text);
	}

	private void OnLoad(object objArg0, EventArgs arg)
	{
		tbHardwareID.Text = ApplicationManager.strHardwareId;
		Text += ApplicationManager.version;
		base.ActiveControl = tbUserName;
	}

	private void OnEnterGroupBox1(object objArg0, EventArgs arg)
	{
	}

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
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LicenseForm));
		lblUserName = new Label();
		lblSerialKey = new Label();
		lblHardwareId = new Label();
		tbHardwareID = new TextBox();
		tbUserName = new TextBox();
		tbSerialKey = new TextBox();
		btnActivate = new Button();
		btnClose = new Button();
		btnCopy = new Button();
		groupBox1 = new GroupBox();
		pictureBox2 = new PictureBox();
		groupBox1.SuspendLayout();
		((ISupportInitialize)pictureBox2).BeginInit();
		SuspendLayout();

		lblUserName.AutoSize = true;
		lblUserName.BackColor = Color.Transparent;
		lblUserName.Location = new Point(11, 22);
        lblUserName.Name = "label1";
		lblUserName.Size = new Size(63, 13);
		lblUserName.TabIndex = 0;
        lblUserName.Text = "User Name:";

		lblSerialKey.AutoSize = true;
		lblSerialKey.BackColor = Color.Transparent;
		lblSerialKey.Location = new Point(11, 48);
        lblSerialKey.Name = "label2";
		lblSerialKey.Size = new Size(57, 13);
		lblSerialKey.TabIndex = 1;
        lblSerialKey.Text = "Serial Key:";

		lblHardwareId.AutoSize = true;
		lblHardwareId.BackColor = Color.Transparent;
        lblHardwareId.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 204);
		lblHardwareId.Location = new Point(12, 81);
        lblHardwareId.Name = "label3";
		lblHardwareId.Size = new Size(82, 13);
		lblHardwareId.TabIndex = 2;
        lblHardwareId.Text = "Hardware ID:";

        tbHardwareID.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 204);
		tbHardwareID.ForeColor = SystemColors.WindowText;
		tbHardwareID.Location = new Point(100, 78);
		tbHardwareID.Name = "tbHardwareID";
		tbHardwareID.ReadOnly = true;
		tbHardwareID.Size = new Size(298, 20);
		tbHardwareID.TabIndex = 3;

		tbUserName.Location = new Point(96, 19);
		tbUserName.MaxLength = 50;
		tbUserName.Name = "tbUserName";
		tbUserName.Size = new Size(298, 20);
		tbUserName.TabIndex = 4;

		tbSerialKey.Location = new Point(96, 45);
		tbSerialKey.MaxLength = 50;
		tbSerialKey.Name = "tbSerialKey";
		tbSerialKey.Size = new Size(298, 20);
		tbSerialKey.TabIndex = 5;

		btnActivate.Location = new Point(410, 136);
		btnActivate.Name = "bActivate";
		btnActivate.Size = new Size(70, 23);
		btnActivate.TabIndex = 6;
		btnActivate.Text = "Activate";
		btnActivate.UseVisualStyleBackColor = true;
		btnActivate.Click += OnClickBtnActivate;

		btnClose.Location = new Point(410, 162);
		btnClose.Name = "bClose";
		btnClose.Size = new Size(70, 23);
		btnClose.TabIndex = 7;
		btnClose.Text = "Close";
		btnClose.UseVisualStyleBackColor = true;
		btnClose.Click += OnClickBtnClose;

		btnCopy.Location = new Point(410, 77);
		btnCopy.Name = "bCopy";
		btnCopy.Size = new Size(71, 23);
		btnCopy.TabIndex = 8;
		btnCopy.Text = "Copy";
		btnCopy.UseVisualStyleBackColor = true;
		btnCopy.Click += OnClickBtnCopy;

		groupBox1.BackColor = Color.Transparent;
		groupBox1.Controls.Add(tbUserName);
		groupBox1.Controls.Add(lblUserName);
		groupBox1.Controls.Add(lblSerialKey);
		groupBox1.Controls.Add(tbSerialKey);
		groupBox1.Location = new Point(4, 119);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new Size(400, 82);
		groupBox1.TabIndex = 10;
		groupBox1.TabStop = false;
		groupBox1.Text = "Please, enter your license details bellow:";
		groupBox1.Enter += OnEnterGroupBox1;

		pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		pictureBox2.Image = AppResourceManager.GetPaymentPattern1Bmp();
		pictureBox2.Location = new Point(-3, 0);
		pictureBox2.Name = "pictureBox2";
		pictureBox2.Size = new Size(497, 66);
		pictureBox2.TabIndex = 12;
		pictureBox2.TabStop = false;

		base.AutoScaleDimensions = new SizeF(6f, 13f);
		base.AutoScaleMode = AutoScaleMode.Font;
		BackgroundImage = AppResourceManager.GetWallpaperBitmap();
		base.ClientSize = new Size(492, 210);
		base.Controls.Add(pictureBox2);
		base.Controls.Add(groupBox1);
		base.Controls.Add(btnCopy);
		base.Controls.Add(btnClose);
		base.Controls.Add(btnActivate);
		base.Controls.Add(tbHardwareID);
		base.Controls.Add(lblHardwareId);
		base.FormBorderStyle = FormBorderStyle.FixedSingle;
		base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "fActivation";
		base.StartPosition = FormStartPosition.CenterScreen;
		Text = "Activation: Filter v";
		base.Load += OnLoad;
		groupBox1.ResumeLayout(performLayout: false);
		groupBox1.PerformLayout();
		((ISupportInitialize)pictureBox2).EndInit();
		ResumeLayout(performLayout: false);
		PerformLayout();
	}
}
