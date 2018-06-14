﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace school_management_system
{
    public partial class FrmBusFeePayment : Form
    {
        SqlDataReader rdr = null;
        DataTable dtable = new DataTable();
        SqlConnection con = null;

        DataSet ds = new DataSet();
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        Connectionstring cs = new Connectionstring();
        clsFunc cf = new clsFunc();
        string st1;
        string st2;
        public FrmBusFeePayment()
        {
            InitializeComponent();
        }
        private void AutocompleScholaNo()
        {
            try
            {

                con = new SqlConnection(cs.DBcon);
                con.Open();


                string ct = "select distinct RTRIM(AdmissionNo) from BusHolders ";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdmissionNo.Items.Add(rdr[0]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reset()
        {
            FeePaymentID.Text = "";
            AdmissionNo.Text = "";
            StudentName.Text = "";
            txtclass.Text = "";
            txtSection.Text = "";

            txtBusCharges.Text = "";
            txtSourceLocation.Text = "";
            txtModeOfPayment.Text = "";
            PaymentDate.Text = DateTime.Today.ToString();
            txtPaymentModeDetails.Text = "";
            txtFine.Text = "";
            txtTotalPaid.Text = "";
            txtDueFees.Text = "";
            btnSave.Enabled = true;
            Delete.Enabled = false;
            Update_record.Enabled = false;
            Print.Enabled = false;
            AdmissionNo.Focus();
        }
        private void FrmBusFeePayment_Load(object sender, EventArgs e)
        {
            AutocompleScholaNo();
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        private void auto()
        {
            FeePaymentID.Text = "F-" + GetUniqueKey(7);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (AdmissionNo.Text == "")
                {
                    MessageBox.Show("Please select Admission No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AdmissionNo.Focus();
                    return;
                }


                if (txtModeOfPayment.Text == "")
                {
                    MessageBox.Show("Please select mode of payment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtModeOfPayment.Focus();
                    return;
                }
                if (txtTotalPaid.Text == "")
                {
                    MessageBox.Show("Please enter total paid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalPaid.Focus();
                    return;
                }
               
                con = new SqlConnection(cs.DBcon);
                con.Open();
                string ct = "select AdmissionNo from BusFeePayment where AdmissionNo= '" +AdmissionNo.Text +  "'";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;

                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("Record Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Reset();

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }

                if (AdmissionNo.Text == "")
                {
                    MessageBox.Show("Please select Admission No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AdmissionNo.Focus();
                    return;
                }


                if (txtModeOfPayment.Text == "")
                {
                    MessageBox.Show("Please select mode of payment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtModeOfPayment.Focus();
                    return;
                }
                if (txtTotalPaid.Text == "")
                {
                    MessageBox.Show("Please enter total paid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalPaid.Focus();
                    return;
                }


                auto();

                con = new SqlConnection(cs.DBcon);
                con.Open();

                string cb = "insert into BusFeePayment(FeePaymentID,AdmissionNo,BusCharges,DateOfPayment,ModeOfPayment,PaymentModeDetails,TotalPaid,Fine,DueFees) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8,@d9)";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1",FeePaymentID.Text.Trim());
                cmd.Parameters.AddWithValue("@d2" ,AdmissionNo.Text.Trim());
                cmd.Parameters.AddWithValue("@d3", Convert.ToInt32(txtBusCharges.Text));
                cmd.Parameters.AddWithValue("@d4" ,(PaymentDate.Text));
                cmd.Parameters.AddWithValue("@d5" ,(txtModeOfPayment.Text));
                cmd.Parameters.AddWithValue("@d6", txtPaymentModeDetails.Text);
                 cmd.Parameters.AddWithValue("@d7",Convert.ToInt32(txtTotalPaid.Text));
                if (txtFine.Text == "")
                {
                    cmd.Parameters.AddWithValue("@d8",0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@d8",Convert.ToInt32(txtFine.Text));
                }

                cmd.Parameters.AddWithValue("@d9", Convert.ToInt32(txtDueFees.Text));
           
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                st1 = lblUser.Text;
                st2 = "Bus Fee Paid '" + AdmissionNo.Text + "' having StudentName='" +StudentName.Text+ "'";
                cf.LogFunc(st1, System.DateTime.Now, st2);
                btnSave.Enabled = false;
                Print.Enabled = true;

                con.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Update_record_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBcon);
                con.Open();

                string cb = "update BusFeePayment set AdmissionNo=@d2,BusCharges=@d3,DateOfPayment=@d4,ModeOfPayment=@d5,PaymentModeDetails=@d6,TotalPaid=@d7,Fine=@d8,DueFees=@d9 where FeePaymentID=@d1";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@d1", System.Data.SqlDbType.NChar, 20, "FeePaymentID"));

                cmd.Parameters.Add(new SqlParameter("@d2", System.Data.SqlDbType.NChar, 15, "AdmissionNo"));

                cmd.Parameters.Add(new SqlParameter("@d3", System.Data.SqlDbType.Int, 10, "BusCharges"));

                cmd.Parameters.Add(new SqlParameter("@d4", System.Data.SqlDbType.NChar, 30, "DateOfPayment"));

                cmd.Parameters.Add(new SqlParameter("@d5", System.Data.SqlDbType.NChar, 20, "ModeOfPayment"));
                cmd.Parameters.Add(new SqlParameter("@d6", System.Data.SqlDbType.VarChar, 200, "PaymentModeDetails"));
                cmd.Parameters.Add(new SqlParameter("@d7", System.Data.SqlDbType.Int, 10, "TotalPaid"));

                cmd.Parameters.Add(new SqlParameter("@d8", System.Data.SqlDbType.Int, 10, "Fine"));
                cmd.Parameters.Add(new SqlParameter("@d9", System.Data.SqlDbType.Int, 10, "DueFees"));
                cmd.Parameters["@d1"].Value = FeePaymentID.Text.Trim();
                cmd.Parameters["@d2"].Value = AdmissionNo.Text.Trim();
                cmd.Parameters["@d3"].Value = Convert.ToInt32(txtBusCharges.Text);
                cmd.Parameters["@d4"].Value = (PaymentDate.Text);
                cmd.Parameters["@d5"].Value = (txtModeOfPayment.Text);
                cmd.Parameters["@d6"].Value = (txtPaymentModeDetails.Text);
                cmd.Parameters["@d7"].Value = Convert.ToInt32(txtTotalPaid.Text);
                if (txtFine.Text == "")
                {
                    cmd.Parameters["@d8"].Value = 0;
                }
                else
                {
                    cmd.Parameters["@d8"].Value = Convert.ToInt32(txtFine.Text);
                }

                cmd.Parameters["@d9"].Value = Convert.ToInt32(txtDueFees.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                st1 = lblUser.Text;
                st2 = "Paid Bus Fee Updated  having StudentName='" +StudentName.Text+ "'";
                cf.LogFunc(st1, System.DateTime.Now, st2);
                Update_record.Enabled = false;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
           
            try
            {


                int RowsAffected = 0;

                con = new SqlConnection(cs.DBcon);

                con.Open();


                string cq = "delete from BusFeePayment where FeePaymentID=@DELETE1;";


                cmd = new SqlCommand(cq);

                cmd.Connection = con;

                cmd.Parameters.Add(new SqlParameter("@DELETE1", System.Data.SqlDbType.NChar, 20, "FeePaymentID"));


                cmd.Parameters["@DELETE1"].Value = FeePaymentID.Text;
                RowsAffected = cmd.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    st1 = lblUser.Text;
                    st2 = " Paid Bus Fee Deleted having FeePaymentID='" + FeePaymentID.Text + "'";
                    cf.LogFunc(st1, System.DateTime.Now, st2);
                    Delete.Enabled = false;

                    Reset();
                }
                else
                {
                    MessageBox.Show("No Record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    Reset();
                }
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdmissionNo_SelectedIndexChanged(object sender, EventArgs e)
        {
             try
            {
                con = new SqlConnection(cs.DBcon);

                con.Open();
                cmd = con.CreateCommand();

                cmd.CommandText = "SELECT StudentName,Class,Section,BusHolders.SourceLocation,BusCharges FROM student,BusHolders,Transportation WHERE Student.AdmissionNo=BusHolders.AdmissionNo and BusHolders.SourceLocation=Transportation.SourceLocation and Student.AdmissionNo = '" +AdmissionNo.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {


                    StudentName.Text = rdr.GetString(0).Trim();


                    txtclass.Text= rdr.GetString(1).Trim();

                   txtSection.Text = rdr.GetString(2).Trim();
                    txtSourceLocation.Text = rdr.GetString(3).Trim();
                    txtBusCharges.Text = rdr.GetInt32(4).ToString();
                }


                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtTotalPaid_TextChanged(object sender, EventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int val3 = 0;
            int.TryParse(txtBusCharges.Text, out val1);
            int.TryParse(txtFine.Text, out val2);
            int.TryParse(txtTotalPaid.Text, out val3);

            int I = ((val1 + val2) - val3);

            txtDueFees.Text = I.ToString();
        }

        private void txtTotalPaid_Validating(object sender, CancelEventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int val3 = 0;
            int.TryParse(txtBusCharges.Text, out val1);
            int.TryParse(txtFine.Text, out val2);
            int.TryParse(txtTotalPaid.Text, out val3);
            if (val3 > val1 + val2)
            {
                MessageBox.Show("Total Paid can not be more than Transportation Fees + Fine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTotalPaid.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmBusFeePaymentRecordcs frm = new FrmBusFeePaymentRecordcs();
            frm.lblUser.Text = lblUser.Text;
            frm.lblUserType.Text = lblUserType.Text;
            frm.Show();
        }

        private void txtFine_TextChanged(object sender, EventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int val3 = 0;
            int.TryParse(txtBusCharges.Text, out val1);
            int.TryParse(txtFine.Text, out val2);
            int.TryParse(txtTotalPaid.Text, out val3);

            int I = ((val1 + val2) - val3);

            txtDueFees.Text = I.ToString();
        }

        private void FrmBusFeePayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frm_Main_Menu frm = new frm_Main_Menu();
            frm.UserType.Text = lblUserType.Text;
            frm.User.Text = lblUser.Text;
            frm.Show();
        }

        private void Print_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                FrmBusFeeReceipt frm = new FrmBusFeeReceipt();
                RptBusFeePaymentReceipt rpt = new RptBusFeePaymentReceipt();
                //The report you created.
                SqlConnection myConnection = default(SqlConnection);
                SqlCommand MyCommand = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                ERPS_DBDataSet myDS = new ERPS_DBDataSet();
                //The DataSet you created.


                myConnection = new SqlConnection(cs.DBcon);
                MyCommand.Connection = myConnection;
                MyCommand.CommandText = "select *  from BusFeePayment,Student,Transportation,BusHolders where Student.AdmissionNo=BusHolders.AdmissionNo and BusFeePayment.AdmissionNo=Student.AdmissionNo and Transportation.SourceLocation=BusHolders.SourceLocation and FeePaymentID= '" + FeePaymentID.Text + "'";
                MyCommand.CommandType = CommandType.Text;
                myDA.SelectCommand = MyCommand;
                myDA.Fill(myDS, "BusFeePayment");
                myDA.Fill(myDS, "Transportation");
                myDA.Fill(myDS, "BusHolders");
                myDA.Fill(myDS, "Student");
                rpt.SetDataSource(myDS);
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }
        }
    }

