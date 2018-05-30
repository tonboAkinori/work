using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

class MainProgram
{
	static void Main()
	{
		//Mutex名を決める
		string mutexName = "MyApplicationClock";
		bool createdNew;

    	//Mutexオブジェクトを作成する
    	Mutex mutex = new Mutex(true, mutexName, out createdNew);
    	
    	if (createdNew == false)
    	{
    		MessageBox.Show("多重起動は許可されていません");
    		mutex.Close();
    		
    		return;
    	}
    	
    	try
    	{
    		Form form = new MainForm();
    		Application.Run(form);
    	}
    	finally
    	{
    		mutex.ReleaseMutex();
    		mutex.Close();
    	}
    }
}

class MainForm : Form
{
	private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
	private Label label = new Label();
	private Point mousePosition;
	
	public MainForm()
	{
		this.FormBorderStyle = FormBorderStyle.None;
		this.ShowInTaskbar = false;
		
		timer.Interval = 100;
		
		timer.Tick += new EventHandler(updateTime);
		timer.Start();
		
		label.Text = DateTime.Now.ToString("HH:mm:ss");
		label.Font = new Font("Brodway", 35);
		label.AutoSize = true;
		
		this.Height = 45;
		this.Width = 250;
		
		this.TransparencyKey = this.BackColor;
		
		label.MouseDown += new MouseEventHandler(Timer_MouseDown);
		label.MouseUp += new MouseEventHandler(Timer_MouseMove);
		label.ForeColor = Color.DarkCyan;
		Controls.Add(label);
		
		this.TopMost = true; //常に最前面表示
	}
	
	private void updateTime(object sender, EventArgs e)
	{
		label.Text = DateTime.Now.ToString("HH:mm:ss");
	}
	
	private void Timer_MouseDown(object sender, MouseEventArgs e)
	{
		if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
		{
			//位置を記憶する
			mousePosition = new Point(e.X, e.Y);
		}
	}
	
	private void Timer_MouseMove(object sender, MouseEventArgs e)
	{
		if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
		{
			//移動
			this.Left += e.X - mousePosition.X;
			this.Top += e.Y - mousePosition.Y;
		}
	}
}