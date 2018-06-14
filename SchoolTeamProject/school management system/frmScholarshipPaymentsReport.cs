﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace school_management_system
{
    public partial class frmScholarshipPaymentsReport : Form
    {
        SqlDataReader rdr = null;
        DataTable dtable = new DataTable();
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds = new DataSet();
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        Connectionstring cs = new Connectionstring();
        public frmScholarshipPaymentsReport()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }
        private void AutocompleteScholarNo()
        {

            try
            {

                SqlConnection CN = new SqlConnection(cs.DBcon);

                CN.Open();
                adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("SELECT distinct RTRIM(AdmissionNo) FROM ScholarshipPayment", CN);
                ds = new DataSet("ds");

                adp.Fill(ds);
                dtable = ds.Tables[0];
               // AdmissionNo.Items.Clear();

                foreach (DataRow drow in dtable.Rows)
                {
                    AdmissionNo.Items.Add(drow[0].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AutocompleteCourse()
        {

            try
            {

                SqlConnection CN = new SqlConnection(cs.DBcon);

                CN.Open();
                adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("SELECT distinct RTRIM(Class) FROM ScholarshipPayment,Student where Student.AdmissionNo=ScholarshipPayment.AdmissionNo", CN);
                ds = new DataSet("ds");
                adp.Fill(ds);
                dtable = ds.Tables[0];
                Class.Items.Clear();

                foreach (DataRow drow in dtable.Rows)
                {
                    Class.Items.Add(drow[0].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Course_SelectedIndexChanged(object sender, EventArgs e)
        {
            Section.Items.Clear();
            Section.Text = "";
            Section.Enabled = true;

            try
            {

                con = new SqlConnection(cs.DBcon);
                con.Open();
                string ct = "select distinct RTRIM(Section) from ScholarshipPayment,Student where Student.AdmissionNo=ScholarshipPayment.AdmissionNo and class= '" + Class.Text + "'";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Section.Items.Add(rdr[0]);
                }
                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Class.Text == "")
            {
                MessageBox.Show("Please select class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Class.Focus();
                return;
            }
            if (Section.Text == "")
            {
                MessageBox.Show("Please select Section", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Section.Focus();
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
               RptScholarshipPaymentReport rpt = new RptScholarshipPaymentReport();
                //The report you created.
                SqlConnection myConnection = default(SqlConnection);
                SqlCommand MyCommand = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                ERPS_DBDataSet myDS = new ERPS_DBDataSet();
                //The DataSet you created.
               myConnection = new SqlConnection(cs.DBcon);
                MyCommand.Connection = myConnection;
                MyCommand.CommandText = "select *  from ScholarshipPayment,Student,Scholarship where Student.AdmissionNo=ScholarshipPayment.AdmissionNo and ScholarShipPayment.ScholarshipID=Scholarship.ScholarShipID and PaymentDate between @date1 and @date2 and Class= '" + Class.Text + "'and Section='" + Section.Text + "' order by PaymentDate ";
                MyCommand.Parameters.Add("@date1", SqlDbType.DateTime, 30, "PaymentDate").Value = Date_from.Value.Date;
                MyCommand.Parameters.Add("@date2", SqlDbType.DateTime, 30, "PaymentDate").Value = Date_to.Value.Date;
                MyCommand.CommandType = CommandType.Text;
                myDA.SelectCommand = MyCommand;
                myDA.Fill(myDS, "ScholarshipPayment");
                myDA.Fill(myDS, "Student");
                myDA.Fill(myDS, "Scholarship");
                rpt.SetDataSource(myDS);

                crystalReportViewer1.ReportSource = rpt;

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
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                RptScholarshipPaymentReport rpt = new RptScholarshipPaymentReport();
                //The report you created.
                SqlConnection myConnection = default(SqlConnection);
                SqlCommand MyCommand = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                ERPS_DBDataSet myDS = new ERPS_DBDataSet();
                //The DataSet you created.


                myConnection = new SqlConnection(cs.DBcon);
                MyCommand.Connection = myConnection;
                MyCommand.CommandText = "select *  from ScholarshipPayment,Student,Scholarship where Student.AdmissionNo=ScholarshipPayment.AdmissionNo and ScholarShipPayment.ScholarshipID=Scholarship.ScholarShipID and scholarshippayment.admissionNo='"+AdmissionNo.Text+"' order by PaymentDate ";
               
                MyCommand.CommandType = CommandType.Text;
                myDA.SelectCommand = MyCommand;
                myDA.Fill(myDS, "ScholarshipPayment");
                myDA.Fill(myDS, "Student");
                myDA.Fill(myDS, "Scholarship");
                rpt.SetDataSource(myDS);

                crystalReportViewer2.ReportSource = rpt;  

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
             try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                RptScholarshipPaymentReport rpt = new RptScholarshipPaymentReport();
                //The report you created.
                SqlConnection myConnection = default(SqlConnection);
                SqlCommand MyCommand = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                ERPS_DBDataSet myDS = new ERPS_DBDataSet();
                //The DataSet you created.


                myConnection = new SqlConnection(cs.DBcon);
                MyCommand.Connection = myConnection;
                MyCommand.CommandText = "select *  from ScholarshipPayment,Student,Scholarship where Student.AdmissionNo=ScholarshipPayment.AdmissionNo and ScholarShipPayment.ScholarshipID=Scholarship.ScholarShipID and PaymentDate between @date1 and @date2  order by PaymentDate ";
                MyCommand.Parameters.Add("@date1", SqlDbType.DateTime, 30, "PaymentDate").Value = PaymentDateFrom.Value.Date;
                MyCommand.Parameters.Add("@date2", SqlDbType.DateTime, 30, "PaymentDate").Value = PaymentDateTo.Value.Date;

                MyCommand.CommandType = CommandType.Text;
                myDA.SelectCommand = MyCommand;
                myDA.Fill(myDS, "ScholarshipPayment");
                myDA.Fill(myDS, "Student");
                myDA.Fill(myDS, "Scholarship");
                rpt.SetDataSource(myDS);

                crystalReportViewer3.ReportSource = rpt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                RptScholarshipPaymentReport rpt = new RptScholarshipPaymentReport();
                //The report you created.
                SqlConnection myConnection = default(SqlConnection);
                SqlCommand MyCommand = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                ERPS_DBDataSet myDS = new ERPS_DBDataSet();
                //The DataSet you created.


                myConnection = new SqlConnection(cs.DBcon);
                MyCommand.Connection = myConnection;
                MyCommand.CommandText = "select *  from ScholarshipPayment,Student,Scholarship where Student.AdmissionNo=ScholarshipPayment.AdmissionNo and ScholarShipPayment.ScholarshipID=Scholarship.ScholarShipID and PaymentDate between @date1 and @date2 and  DuePayment > 0 order by PaymentDate ";
                MyCommand.Parameters.Add("@date1", SqlDbType.DateTime, 30, "PaymentDate").Value = DateFrom.Value.Date;
                MyCommand.Parameters.Add("@date2", SqlDbType.DateTime, 30, "PaymentDate").Value = DateTo.Value.Date;
                MyCommand.CommandType = CommandType.Text;
                myDA.SelectCommand = MyCommand;
                myDA.Fill(myDS, "ScholarshipPayment");
                myDA.Fill(myDS, "Student");
                myDA.Fill(myDS, "Scholarship");
                rpt.SetDataSource(myDS);

                crystalReportViewer4.ReportSource = rpt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class.Text = "";
            Section.Text = "";
            Date_from.Text = System.DateTime.Today.ToString();
            Date_to.Text = System.DateTime.Today.ToString();
            crystalReportViewer1.ReportSource = null;
            Section.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AdmissionNo.Text = "";
            crystalReportViewer2.ReportSource = null;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PaymentDateFrom.Text = System.DateTime.Today.ToString();
            PaymentDateTo.Text = System.DateTime.Today.ToString();
            crystalReportViewer3.ReportSource = null;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DateFrom.Text = System.DateTime.Today.ToString();
            DateTo.Text = System.DateTime.Today.ToString();
            crystalReportViewer4.ReportSource = null;
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            Class.Text = "";
            Section.Text = "";
            Date_from.Text = System.DateTime.Today.ToString();
            Date_to.Text = System.DateTime.Today.ToString();
            crystalReportViewer1.ReportSource = null;
            AdmissionNo.Text = "";
            crystalReportViewer2.ReportSource = null;
            PaymentDateFrom.Text = System.DateTime.Today.ToString();
            PaymentDateTo.Text = System.DateTime.Today.ToString();
            crystalReportViewer3.ReportSource = null;
            DateFrom.Text = System.DateTime.Today.ToString();
            DateTo.Text = System.DateTime.Today.ToString();
            crystalReportViewer4.ReportSource = null;
        }

        private void frmScholarshipPaymentsReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frm_Main_Menu frm = new frm_Main_Menu();
            frm.UserType.Text = lblUserType.Text;
            frm.User.Text = lblUser.Text;
            frm.Show();
        }

        private void frmScholarshipPaymentsReport_Load(object sender, EventArgs e)
        {
            AutocompleteCourse();
            AutocompleteScholarNo();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
