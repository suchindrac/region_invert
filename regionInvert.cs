using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace RegionInvert
{
    /*
     * Create a class which is a Child of Form
     * 
     */
    public partial class RegionInvert : Form, IMessageFilter
    {
        private Rectangle rect;
        
        private int toggle = 0;
        int screen_width = SystemInformation.VirtualScreen.Width;
	int screen_height = SystemInformation.VirtualScreen.Height;

	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd);
	
	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr SetFocus(IntPtr hWnd);
	
       	enum KeyModifier
	{
	        None = 0,
	        Alt = 1,
		Control = 2,
		Shift = 4,
		WinKey = 8
	}
	
	public bool PreFilterMessage(ref Message m)
	{
		const int WM_KEYUP = 0x101;
		if (m.Msg == WM_KEYUP)
   		{
   			return true;
   		} else {
   			return false;
   		}
   		
   	}
        
        public RegionInvert()
        {
        	Application.AddMessageFilter(this);
        	
        	int id = 0;

            	/*
             	* Initialize component
             	* 
             	*/
            	InitializeComponent();

            	/*
            	 * Form background, border and transparency
            	 * 
            	 */
		            	    	
            	this.Show();
	}
	
	protected override CreateParams CreateParams
	{
	    get
	    {
		CreateParams createParams = base.CreateParams;
		createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT

		return createParams;
	    }
	}
	
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
	       	switch(keyData)
	       	{
	       		case Keys.T:
	       			if (this.toggle == 0)
	       			{
	       				this.rect.X = PointToScreen(Cursor.Position).X;
	       				this.rect.Y = PointToScreen(Cursor.Position).Y;;
	       				this.toggle = 1;
	       				return true;
	       			}
	       			if (this.toggle == 1)
	       			{
		       			this.rect.Width = PointToScreen(Cursor.Position).X - this.rect.X;
		       			this.rect.Height = PointToScreen(Cursor.Position).Y - this.rect.Y;
		       			this.toggle = 2;
		       			return true;
		       		}
				if (this.toggle == 2)
				{
					InvertRegion();
					this.toggle = 3;
					return true;
				}
				if (this.toggle == 3)
				{
					InvertRegion();
					this.toggle = 0;
					return true;
				}

				break;
            		case Keys.X:
            			this.Close();
            			Application.Exit();
            			break;
		}	
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void InvertRegion()
	{
		if ((this.rect.Width + this.rect.Height) != 0)
		{
			ControlPaint.FillReversibleRectangle(this.rect, Color.White);
	  		ControlPaint.DrawReversibleFrame(this.rect, Color.Black, FrameStyle.Dashed);
	  	}

	  	return;
	}


	#region Windows Form Designer generated code
	
	private void InitializeComponent()
	{

            	// 
            	// Form1
            	// 

            	this.Bounds = Screen.PrimaryScreen.Bounds;
            	this.TopMost = true;
            	this.Size = new Size(screen_width, screen_height);
            	this.BackColor = Color.White;
		this.FormBorderStyle = FormBorderStyle.None;
            	this.TransparencyKey  = Color.Black;

            	this.Opacity = 0;
            	
//            	this.AutoSize = false;
//            	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            	this.ClientSize = new System.Drawing.Size(screen_width, screen_height);
            	this.Name = "Region Invert";
            	this.Text = "Region Invert";
            	this.ResumeLayout(false);
           	this.PerformLayout();
	}	
	
	#endregion

	[STAThread]
	static void Main()
	{
        	Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new RegionInvert());
	}
    }
}
