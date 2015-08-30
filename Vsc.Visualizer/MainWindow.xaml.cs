using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vsc.Core;

namespace Vsc.Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBlock[] addresses;
        private TextBlock[] values;

        private Computer computer;

        public MainWindow()
        {
            InitializeComponent();
            InitAddressesAndValue();
        }

        private void txtInputValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double value;
                if (!double.TryParse(txtInputValue.Text, out value))
                    return;

                if (string.IsNullOrEmpty(txtInputLog.Text))
                    txtInputLog.Text = txtInputValue.Text;
                else
                    txtInputLog.Text = txtInputLog.Text + "\r\n" + txtInputValue.Text;
                txtInputValue.Text = string.Empty;
                txtInputLog.ScrollToEnd();

                txtInputValue.Background = Brushes.White;
                txtInputValue.IsEnabled = false;

                this.computer.IO.Input(value);

                btnStep.IsEnabled = true;
            }
        }

        private void InitAddressesAndValue()
        {
            this.addresses = new TextBlock[]
            {
                txtAddress0, txtAddress1, txtAddress2, txtAddress3, txtAddress4, txtAddress5, txtAddress6, txtAddress7, txtAddress8, txtAddress9,
                txtAddress10, txtAddress11, txtAddress12, txtAddress13, txtAddress14, txtAddress15, txtAddress16, txtAddress17, txtAddress18, txtAddress19,
                txtAddress20, txtAddress21, txtAddress22, txtAddress23, txtAddress24, txtAddress25, txtAddress26, txtAddress27, txtAddress28, txtAddress29,
                txtAddress30, txtAddress31, txtAddress32, txtAddress33, txtAddress34, txtAddress35, txtAddress36, txtAddress37, txtAddress38, txtAddress39,
                txtAddress40, txtAddress41, txtAddress42, txtAddress43, txtAddress44, txtAddress45, txtAddress46, txtAddress47, txtAddress48, txtAddress49,
                txtAddress50, txtAddress51, txtAddress52, txtAddress53, txtAddress54, txtAddress55, txtAddress56, txtAddress57, txtAddress58, txtAddress59,
                txtAddress60, txtAddress61, txtAddress62, txtAddress63, txtAddress64, txtAddress65, txtAddress66, txtAddress67, txtAddress68, txtAddress69,
                txtAddress70, txtAddress71, txtAddress72, txtAddress73, txtAddress74, txtAddress75, txtAddress76, txtAddress77, txtAddress78, txtAddress79,
                txtAddress80, txtAddress81, txtAddress82, txtAddress83, txtAddress84, txtAddress85, txtAddress86, txtAddress87, txtAddress88, txtAddress89,
                txtAddress90, txtAddress91, txtAddress92, txtAddress93, txtAddress94, txtAddress95, txtAddress96, txtAddress97, txtAddress98, txtAddress99
            };
            this.values = new TextBlock[]
            {
                txtData0, txtData1, txtData2, txtData3, txtData4, txtData5, txtData6, txtData7, txtData8, txtData9,
                txtData10, txtData11, txtData12, txtData13, txtData14, txtData15, txtData16, txtData17, txtData18, txtData19,
                txtData20, txtData21, txtData22, txtData23, txtData24, txtData25, txtData26, txtData27, txtData28, txtData29,
                txtData30, txtData31, txtData32, txtData33, txtData34, txtData35, txtData36, txtData37, txtData38, txtData39,
                txtData40, txtData41, txtData42, txtData43, txtData44, txtData45, txtData46, txtData47, txtData48, txtData49,
                txtData50, txtData51, txtData52, txtData53, txtData54, txtData55, txtData56, txtData57, txtData58, txtData59,
                txtData60, txtData61, txtData62, txtData63, txtData64, txtData65, txtData66, txtData67, txtData68, txtData69,
                txtData70, txtData71, txtData72, txtData73, txtData74, txtData75, txtData76, txtData77, txtData78, txtData79,
                txtData80, txtData81, txtData82, txtData83, txtData84, txtData85, txtData86, txtData87, txtData88, txtData89,
                txtData90, txtData91, txtData92, txtData93, txtData94, txtData95, txtData96, txtData97, txtData98, txtData99
            };
        }

        private void btnLoad_Click(object sender, RoutedEventArgs ea)
        {
            var data = OpenFile();
            if (data == null)
                return;

            txtRegAValue.Background = Brushes.White;
            txtRegDValue.Background = Brushes.White;
            txtRegPCValue.Background = Brushes.White;
            txtRegPValue.Background = Brushes.White;
            txtRegXValue.Background = Brushes.White;
            txtRegYValue.Background = Brushes.White;
            txtRegZValue.Background = Brushes.White;
            txtRegIValue.Background = Brushes.White;
            txtRegOValue.Background = Brushes.White;

            txtOutputLog.Background = Brushes.White;

            txtLogMicroActions.Text = string.Empty;

            txtInputValue.Background = Brushes.White;
            txtInputValue.IsEnabled = false;

            foreach (var txtData in this.values)
            {
                txtData.Text = "0";
                txtData.FontWeight = FontWeights.Normal;
                txtData.Background = Brushes.White;
            }

            foreach (var txtAddress in this.addresses)
            {
                txtAddress.Background = Brushes.White;
            }
            txtAddress0.Background = Brushes.LightBlue;

            for (int i = 0; i < data.Length; i++)
            {
                this.values[i].Text = data[i].ToString();
                this.values[i].FontWeight = FontWeights.Bold;
            }

            txtRegAValue.Text = "0";
            txtRegDValue.Text = "0";
            txtRegPCValue.Text = "0";
            txtRegPValue.Text = "0";
            txtRegXValue.Text = "0";
            txtRegYValue.Text = "0";
            txtRegZValue.Text = "0";
            txtRegIValue.Text = "0";
            txtRegOValue.Text = "0";

            txtInputLog.Text = string.Empty;

            txtOutputLog.Text = string.Empty;

            this.computer = new Computer(100, data);

            this.computer.IO.InputRequested += (s, e) =>
            {
                txtInputValue.IsEnabled = true;
                txtInputValue.Focus();
                txtInputValue.Background = Brushes.Pink;
                btnStep.IsEnabled = false;
            };
            this.computer.IO.OutputPerformed += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtOutputLog.Text))
                    txtOutputLog.Text = e.Value.ToString();
                else
                    txtOutputLog.Text = txtOutputLog.Text + "\r\n" + e.Value.ToString();
                txtOutputLog.Background = Brushes.Yellow;
            };
            this.computer.Memory.CellValueChanged += (s, e) =>
            {
                this.values[e.Address].Text = e.NewValue.ToString();
                this.values[e.Address].Background = Brushes.Yellow;
                this.values[e.Address].FontWeight = FontWeights.Bold;
            };
            this.computer.Registers.RegisterValueChanged += (s, e) =>
            {
                switch (e.Register)
                {
                    case Register.A:
                        txtRegAValue.Text = e.NewValue.ToString();
                        txtRegAValue.Background = Brushes.Yellow;
                        break;

                    case Register.D:
                        txtRegDValue.Text = e.NewValue.ToString();
                        txtRegDValue.Background = Brushes.Yellow;
                        break;

                    case Register.I:
                        txtRegIValue.Text = e.NewValue.ToString();
                        txtRegIValue.Background = Brushes.Yellow;
                        break;

                    case Register.O:
                        txtRegOValue.Text = e.NewValue.ToString();
                        txtRegOValue.Background = Brushes.Yellow;
                        break;

                    case Register.PC:
                        txtRegPCValue.Text = e.NewValue.ToString();
                        txtRegPCValue.Background = Brushes.Yellow;
                        foreach (var txtAddress in this.addresses)
                        {
                            txtAddress.Background = Brushes.White;
                        }
                        this.addresses[(int)this.computer.Registers[Register.PC]].Background = Brushes.LightBlue;
                        break;

                    case Register.P:
                        txtRegPValue.Text = e.NewValue.ToString();
                        txtRegPValue.Background = Brushes.Yellow;
                        break;

                    case Register.X:
                        txtRegXValue.Text = e.NewValue.ToString();
                        txtRegXValue.Background = Brushes.Yellow;
                        break;

                    case Register.Y:
                        txtRegYValue.Text = e.NewValue.ToString();
                        txtRegYValue.Background = Brushes.Yellow;
                        break;

                    case Register.Z:
                        txtRegZValue.Text = e.NewValue.ToString();
                        txtRegZValue.Background = Brushes.Yellow;
                        break;
                }
            };
            this.computer.NotifyMicroAction += (s, e) =>
            {
                if (string.IsNullOrEmpty(this.txtLogMicroActions.Text))
                {
                    txtLogMicroActions.Text = e.ActionDescription;
                }
                else
                {
                    txtLogMicroActions.Text += "   =>   " + e.ActionDescription;
                }
            };

            this.btnStep.IsEnabled = true;
        }

        private double[] OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Compiled Files (*.bin)|*.bin|Assembler Files (*.asm)|*.asm"
            };

            if (openFileDialog.ShowDialog() != true)
                return null;

            var fileText = File.ReadAllText(openFileDialog.FileName);
            if (openFileDialog.FileName.EndsWith(".bin"))
            {
                var tokens = fileText.Split(' ');
                var data = new double[tokens.Length];
                for (int i = 0; i < tokens.Length; i++)
                {
                    data[i] = double.Parse(tokens[i]);
                }

                foreach (var txtData in this.values)
                {
                    txtData.Background = Brushes.White;
                }

                return data; 
            }

            return Assembler.Assembler.Compile(fileText);
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            foreach (var txtData in this.values)
            {
                txtData.Background = Brushes.White;
            }

            txtRegAValue.Background = Brushes.White;
            txtRegDValue.Background = Brushes.White;
            txtRegPCValue.Background = Brushes.White;
            txtRegPValue.Background = Brushes.White;
            txtRegXValue.Background = Brushes.White;
            txtRegYValue.Background = Brushes.White;
            txtRegZValue.Background = Brushes.White;
            txtRegIValue.Background = Brushes.White;
            txtRegOValue.Background = Brushes.White;

            txtOutputLog.Background = Brushes.White;

            txtLogMicroActions.Text = string.Empty;

            this.computer.Step();

            if (this.computer.IsStopped)
                this.btnStep.IsEnabled = false;
        }
    }
}
