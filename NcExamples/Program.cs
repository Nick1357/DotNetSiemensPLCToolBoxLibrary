using DotNetSiemensPLCToolBoxLibrary.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NcExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Debugger.Break();
            WriteNck();
            System.Diagnostics.Debugger.Break();
            ReadFromNck();
            System.Diagnostics.Debugger.Break();
            UploadNcFile();
            System.Diagnostics.Debugger.Break();
            DownloadNcFile();
            System.Diagnostics.Debugger.Break();
        }

        private static void ReadFromNck(string ipAddress = "192.168.214.1")
        {
            using (var con = new PLCConnection("ReadFromNck"))
            {
                con.Configuration.CpuIP = ipAddress;
                con.Configuration.CpuSlot = 4;
                con.Connect();

                try
                {
                    if (!con.Connected)
                        con.Connect();

                    #region Channel 1 R[0]
                    var R0 = con.ReadValue(new NC_Var(0x82, 0x41, 0x1, 0x1, 0x15, 0x1, 0xF, 0x8));
                    Console.WriteLine("R0: {0}", R0);
                    #endregion

                    #region List of R-Parameter
                    var rpa = new NC_Var(0x82, 0x40, 0x1, 0x0, 0x15, 0x1, 0xF, 0x8);
                    var tags = new List<PLCNckTag>();
                    int channel = 1;
                    for (int i = 1; i < 10; i++)
                    {
                        tags.Add(rpa.GetNckTag(channel, i));
                        tags.Last().Tag = string.Format("R[{0}]", i - 1);
                    }
                    con.ReadValues(tags);
                    tags.ForEach(f => Console.WriteLine("{0}: {1}", f.Tag, f.Value));
                    #endregion
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        private static void WriteNck(string ipAddress = "192.168.214.1")
        {
            using (var con = new PLCConnection("WriteNck"))
            {
                con.Configuration.CpuIP = ipAddress;
                con.Configuration.CpuSlot = 4;
                con.Connect();

                try
                {
                    if (!con.Connected)
                        con.Connect();

                    #region Channel 1 R[0]
                    var R0 = new NC_Var(0x82, 0x41, 0x1, 0x1, 0x15, 0x1, 0xF, 0x8).GetNckTag();
                    R0.Controlvalue = 5;
                    con.WriteValue(R0);
                    #endregion

                    #region List of R-Parameter
                    var rpa = new NC_Var(0x82, 0x40, 0x1, 0x0, 0x15, 0x1, 0xF, 0x8);
                    var tags = new List<PLCNckTag>();
                    int channel = 1;
                    for (int i = 1; i < 10; i++)
                    {
                        tags.Add(rpa.GetNckTag(channel, i));
                        tags.Last().Controlvalue = i;
                    }
                    con.WriteValues(tags);
                    #endregion
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        private static void UploadNcFile(string ipAddress = "192.168.214.1")
        {
            using (var con = new PLCConnection("UploadNcFile"))
            {
                con.Configuration.CpuIP = ipAddress;
                con.Configuration.CpuSlot = 4;
                con.Connect();

                try
                {
                    if (!con.Connected)
                        con.Connect();

                    bool F_XFER = true; // false for system ini files
                    string NcPath = "/_N_CST_DIR/_N_PROG_EVENT_SPF";
                    string sEditFile = F_XFER ? con.UploadNcFile(NcPath, 0, F_XFER) : con.UploadFromNC(NcPath, F_XFER);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        private static void DownloadNcFile(string ipAddress = "192.168.214.1")
        {
            using (var con = new PLCConnection("DownloadNcFile"))
            {
                con.Configuration.CpuIP = ipAddress;
                con.Configuration.CpuSlot = 4;
                con.Connect();

                try
                {
                    if (!con.Connected)
                        con.Connect();

                    string s = "Hallo NC";

                    string NcPath = "/_N_MPF_DIR/_N_MY_TEST_MPF";
                    con.DownloadToNC(NcPath, null, s);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
    }
}
