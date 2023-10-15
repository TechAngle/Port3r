using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

public class PortCheckerApp : Form
{
    private static readonly int[] dangerousPorts = {
        22,    // SSH
        23,    // Telnet
        25,    // SMTP
        80,    // HTTP
        110,   // POP3
        135,   // RPC
        137,   // NetBIOS
        138,   // NetBIOS
        139,   // NetBIOS
        443,   // HTTPS
        1433,  // MS SQL
        1935,  // Adobe Flash Media Server
        3128,  // Squid Proxy
        3306,  // MySQL
        3389,  // RDP
        5060,  // SIP/VoIP
        5061,  // SIP/VoIP
        5432,  // PostgreSQL
        5433,  // PostgreSQL alternative
        5672,  // AMQP
        5900,  // VNC
        5901,  // VNC alternative
        5902,  // Additional VNC
        6660,  // IRC
        6666,  // IRC
        6667,  // IRC
        6668,  // IRC
        6669,  // IRC
        6697,  // IRC with SSL
        8080,  // HTTP proxy
        8088,  // HTTP proxy alternative
        1194,  // OpenVPN
        1514,  // Syslog alternative
        1521,  // Oracle Database
        28015, // Rust game server
        3128,  // Squid Proxy
        3260,  // iSCSI
        3389,  // RDP
        4444,  // Viruses 
    };

    private Button checkButton;
    private TextBox resultText;

    public PortCheckerApp()
    {
        Text = "Port Checker";
        Width = 400;
        Height = 300;

        createInterface();
    }

    public static List<int> GetPorts() {
        IPGlobalProperties props = IPGlobalProperties.GetIPGlobalProperties();
        var cons = props
            .GetActiveTcpConnections()
            .Select(connection => connection.RemoteEndPoint.Port)
            .Distinct()
            .ToList();

        List<int> opened = new List<int>();
        foreach (var port in cons)
        {
            if (dangerousPorts.Contains(port)) {
                opened.Add(port);
            }
        }

        return opened;
    }

    private void createInterface()
    {
        checkButton = new Button
        {
            Text = "Check ports",
            Location = new System.Drawing.Point(10, 10),
            Size = new System.Drawing.Size(150, 30)
        };
        checkButton.Click += checkPorts;

        resultText = new TextBox
        {
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            WordWrap = true,
            Location = new System.Drawing.Point(10, 50),
            Size = new System.Drawing.Size(360, 200),
            ReadOnly = true
        };

        Controls.Add(checkButton);
        Controls.Add(resultText);
    }

    private async void checkPorts(object sender, EventArgs e)
    {
        resultText.Text = "Ожидайте...";

        List<int> openPorts = GetPorts();

        resultText.Text = openPorts.Count == 0
            ? "No 'dangerous' ports!"
            : "Opened dangerous ports:\r\n" + "Port:"+string.Join("\r\n", openPorts);
    }

    public static void Main()
    {
        Application.Run(new PortCheckerApp());
    }
}
