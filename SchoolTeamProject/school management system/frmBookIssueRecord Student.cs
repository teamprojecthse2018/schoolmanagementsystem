﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
namespace school_management_system
{
    public partial class frmBookIssueRecord : Form
    {
        
        DataTable dtable = new DataTable();
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds = new DataSet();
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        Connectionstring cs = new Connectionstring();
        public frmBookIssueRecord()
        {
            InitializeComponent();
        }
      

         private void AutocompleteStudent()
        {

            try
            {


                SqlConnection CN = new SqlConnection(cs.DBcon);

                CN.Open();
                adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("SELECT distinct Studentname FROM Student,StudentBookIssue where Student.AdmissionNo=StudentBookIssue.AdmissionNo", CN);
                ds = new DataSet("ds");

                adp.Fill(ds);
                dtable = ds.Tables[0];
                txtStudent.Items.Clear();

                foreach (DataRow drow in dtable.Rows)
                {
                    txtStudent.Items.Add(drow[0].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

         private void AutocompleteAdmission()
         {

             try
             {


                 SqlConnection CN = new SqlConnection(cs.DBcon);

                 CN.Open();
                 adp = new SqlDataAdapter();
                 adp.SelectCommand = new SqlCommand("SELECT distinct studentBookissue.AdmissionNo FROM Student,StudentBookIssue where Student.AdmissionNo=StudentBookIssue.AdmissionNo", CN);
                 ds = new DataSet("ds");

                 adp.Fill(ds);
                 dtable = ds.Tables[0];
            

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
        private void frmBookIssueRecord_Load(object sender, EventArgs e)
        {
            AutocompleteStudent();
            AutocompleteAdmission();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBcon);

                con.Open();
                cmd = new SqlCommand("SELECT Rtrim(TransactionID) as [Transaction ID],(IssueDate) as [Issue Date], (DueDate) as [Due Date],Rtrim(Book.AccessionNo) as [Accession No],Rtrim(BookTitle) as [Book Title],Rtrim(Author) [Author],Rtrim(Subject) [Subject],Rtrim(ISBN) [ISBN],Rtrim(Edition) [Edition],Rtrim(Student.AdmissionNo) as [AdmissionNo],Rtrim(StudentName) as [Student Name],Rtrim(Student.Class) as [Class],Rtrim(Student.Section) as [Section],Rtrim(studentBookissue.Status) as[status] from Book,StudentBookIssue,Student where Book.AccessionNo=StudentBookIssue.AccessionNo and studentBookIssue.AdmissionNo=Student.AdmissionNo and studentName='" + txtStudent.Text + "' and IssueDate Between @date1 and @date2 order by IssueDate", con);
                cmd.Parameters.Add("@date1", SqlDbType.DateTime, 30, "issueDate").Value = Date_from.Value.Date;
                cmd.Parameters.Add("@date2", SqlDbType.DateTime, 30, "issueDate").Value = Date_to.Value.Date;

                SqlDataAdapter myDA = new SqlDataAdapter(cmd);

                DataSet myDataSet = new DataSet();

                myDA.Fill(myDataSet, "Book");
                myDA.Fill(myDataSet, "Student");
                myDA.Fill(myDataSet, "StudentBookissue");
     
                dataGridView1.DataSource = myDataSet.Tables["Book"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Student"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["StudentBookissue"].DefaultView;
             
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Set1.Text == "Book issue")
                {
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];
                    this.Hide();
                    frmStudentBookIssue frm = new frmStudentBookIssue();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.cmbAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtsubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[10].Value.ToString();
                    frm.cmbadmissionno.Text = dr.Cells[9].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtsection.Text = dr.Cells[12].Value.ToString();
                   
                        frm.btnDelete.Enabled = true;
                        frm.btnUpdate_record.Enabled = true;
                        frm.lblUser.Text = lblUser.Text;
                        frm.lblUserType.Text = lblUserType.Text;
                   
                        frm.btnSave.Enabled = false;
                  
                      
                    }
                

                else
                {
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];
                    this.Hide();
                    frmBookReturn frm = new frmBookReturn();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.txtAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtSubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[9].Value.ToString();
                    frm.AdmissionNo.Text = dr.Cells[10].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtSection.Text = dr.Cells[12].Value.ToString();
                    frm.lblUser.Text = lblUser.Text;
                    frm.lblUserType.Text = lblUserType.Text;
                    frm.btnDelete.Enabled = false;
                    frm.btnUpdate_record.Enabled = false;

                    frm.btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        
        }

        private void frmBookIssueRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Set10.Text == "Book Return")
            {
                this.Hide();
                frmBookReturn frm = new frmBookReturn();
                frm.lblUserType.Text = lblUserType.Text;
                frm.lblUser.Text = lblUser.Text;
                frm.Show();
            }
            else
            {
                this.Hide();
                frmStudentBookIssue frm = new frmStudentBookIssue();
                frm.lblUserType.Text = lblUserType.Text;
                frm.lblUser.Text = lblUser.Text;
                frm.Show();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBcon);

                con.Open();
                cmd = new SqlCommand("SELECT Rtrim(TransactionID) as [Transaction ID],(IssueDate) as [Issue Date],(DueDate) as [Due Date],Rtrim(Book.AccessionNo) as [Accession No],Rtrim(BookTitle) as [Book Title],Rtrim(Author) [Author],Rtrim(Subject) [Subject],Rtrim(ISBN) [ISBN],Rtrim(Edition) [Edition],Rtrim(Student.AdmissionNo) as [AdmissionNo],Rtrim(StudentName) as [Student Name],Rtrim(Student.Class) as [Class],Rtrim(Student.Section) as [Section],Rtrim(studentBookissue.Status) as[status] from Book,StudentBookIssue,Student where Book.AccessionNo=StudentBookIssue.AccessionNo and studentBookIssue.AdmissionNo=Student.AdmissionNo and DueDate Between @date1 and @date2 order by DueDate", con);
                cmd.Parameters.Add("@date1", SqlDbType.DateTime, 30, "DueDate").Value = DateFrom.Value.Date;
                cmd.Parameters.Add("@date2", SqlDbType.DateTime, 30, "DueDate").Value = DateTo.Value.Date;

                SqlDataAdapter myDA = new SqlDataAdapter(cmd);

                DataSet myDataSet = new DataSet();

                myDA.Fill(myDataSet, "Book");
                myDA.Fill(myDataSet, "Student");
                myDA.Fill(myDataSet, "StudentBookissue");

                dataGridView4.DataSource = myDataSet.Tables["Book"].DefaultView;
                dataGridView4.DataSource = myDataSet.Tables["Student"].DefaultView;
                dataGridView4.DataSource = myDataSet.Tables["StudentBookissue"].DefaultView;

                con.Close();
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
                cmd = new SqlCommand("SELECT Rtrim(TransactionID) as [Transaction ID],(IssueDate) as [Issue Date],(DueDate) as [Due Date],Rtrim(Book.AccessionNo) as [Accession No],Rtrim(BookTitle) as [Book Title],Rtrim(Author) [Author],Rtrim(Subject) [Subject],Rtrim(ISBN) [ISBN],Rtrim(Edition) [Edition],Rtrim(Student.AdmissionNo) as [AdmissionNo],Rtrim(StudentName) as [Student Name],Rtrim(Student.Class) as [Class],Rtrim(Student.Section) as [Section],Rtrim(studentBookissue.Status) as[status] from Book,StudentBookIssue,Student where Book.AccessionNo=StudentBookIssue.AccessionNo and studentBookIssue.AdmissionNo=Student.AdmissionNo and studentbookissue.AdmissionNo = '" + AdmissionNo.Text + "'order by IssueDate", con);


                SqlDataAdapter myDA = new SqlDataAdapter(cmd);

                DataSet myDataSet = new DataSet();

                myDA.Fill(myDataSet, "Book");
                myDA.Fill(myDataSet, "Student");
                myDA.Fill(myDataSet, "StudentBookissue");

                dataGridView2.DataSource = myDataSet.Tables["Book"].DefaultView;
                dataGridView2.DataSource = myDataSet.Tables["Student"].DefaultView;
                dataGridView2.DataSource = myDataSet.Tables["StudentBookissue"].DefaultView;

                con.Close();
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
                con = new SqlConnection(cs.DBcon);

                con.Open();
                cmd = new SqlCommand("SELECT Rtrim(TransactionID) as [Transaction ID],(IssueDate) as [Issue Date],(DueDate) as [Due Date],Rtrim(Book.AccessionNo) as [Accession No],Rtrim(BookTitle) as [Book Title],Rtrim(Author) [Author],Rtrim(Subject) [Subject],Rtrim(ISBN) [ISBN],Rtrim(Edition) [Edition],Rtrim(Student.AdmissionNo) as [AdmissionNo],Rtrim(StudentName) as [Student Name],Rtrim(Student.Class) as [Class],Rtrim(Student.Section) as [Section],Rtrim(studentBookissue.Status) as[status] from Book,StudentBookIssue,Student where Book.AccessionNo=StudentBookIssue.AccessionNo and studentBookIssue.AdmissionNo=Student.AdmissionNo and IssueDate Between @date1 and @date2 order by IssueDate", con);
                cmd.Parameters.Add("@date1", SqlDbType.DateTime, 30, "IssueDate").Value = IssueDateFrom.Value.Date;
                cmd.Parameters.Add("@date2", SqlDbType.DateTime, 30, "IssueDate").Value = issueDateTo.Value.Date;

                SqlDataAdapter myDA = new SqlDataAdapter(cmd);

                DataSet myDataSet = new DataSet();

                myDA.Fill(myDataSet, "Book");
                myDA.Fill(myDataSet, "Student");
                myDA.Fill(myDataSet, "StudentBookissue");

                dataGridView3.DataSource = myDataSet.Tables["Book"].DefaultView;
                dataGridView3.DataSource = myDataSet.Tables["Student"].DefaultView;
                dataGridView3.DataSource = myDataSet.Tables["StudentBookissue"].DefaultView;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBcon);

                con.Open();
                cmd = new SqlCommand("SELECT Rtrim(TransactionID) as [Transaction ID],(IssueDate) as [Issue Date],(DueDate) as [Due Date],Rtrim(Book.AccessionNo) as [Accession No],Rtrim(BookTitle) as [Book Title],Rtrim(Author) [Author],Rtrim(Subject) [Subject],Rtrim(ISBN) [ISBN],Rtrim(Edition) [Edition],Rtrim(Student.AdmissionNo) as [AdmissionNo],Rtrim(StudentName) as [Student Name],Rtrim(Student.Class) as [Class],Rtrim(Student.Section) as [Section],Rtrim(studentBookissue.Status) as[status] from Book,StudentBookIssue,Student where Book.AccessionNo=StudentBookIssue.AccessionNo and studentBookIssue.AdmissionNo=Student.AdmissionNo and studentName Like'" + textBox1.Text + "%' order by IssueDate", con);

                SqlDataAdapter myDA = new SqlDataAdapter(cmd);

                DataSet myDataSet = new DataSet();

                myDA.Fill(myDataSet, "Book");
                myDA.Fill(myDataSet, "Student");
                myDA.Fill(myDataSet, "StudentBookissue");

                dataGridView1.DataSource = myDataSet.Tables["Book"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Student"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["StudentBookissue"].DefaultView;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Set1.Text == "Book issue")
                {
                    DataGridViewRow dr = dataGridView2.SelectedRows[0];
                    this.Hide();
                    frmStudentBookIssue frm = new frmStudentBookIssue();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.cmbAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtsubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[10].Value.ToString();
                    frm.cmbadmissionno.Text = dr.Cells[9].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtsection.Text = dr.Cells[12].Value.ToString();
                   
                        frm.btnDelete.Enabled = true;
                        frm.btnUpdate_record.Enabled = true;
                        frm.lblUser.Text = lblUser.Text;
                        frm.lblUserType.Text = lblUserType.Text;

                        frm.btnSave.Enabled = false;
                   
                    }
                

                else
                {
                    DataGridViewRow dr = dataGridView2.SelectedRows[0];
                    this.Hide();
                    frmBookReturn frm = new frmBookReturn();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.txtAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtSubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[9].Value.ToString();
                    frm.AdmissionNo.Text = dr.Cells[10].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtSection.Text = dr.Cells[12].Value.ToString();
                    frm.lblUser.Text = lblUser.Text;
                    frm.lblUserType.Text = lblUserType.Text;
                    frm.btnDelete.Enabled = false;
                    frm.btnUpdate_record.Enabled = false;

                    frm.btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Set1.Text == "Book issue")
                {
                    DataGridViewRow dr = dataGridView3.SelectedRows[0];
                    this.Hide();
                    frmStudentBookIssue frm = new frmStudentBookIssue();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.cmbAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtsubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[10].Value.ToString();
                    frm.cmbadmissionno.Text = dr.Cells[9].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtsection.Text = dr.Cells[12].Value.ToString();
                     frm.btnDelete.Enabled = true;
                        frm.btnUpdate_record.Enabled = true;
                        frm.lblUser.Text = lblUser.Text;
                        frm.lblUserType.Text = lblUserType.Text;

                        frm.btnSave.Enabled = false;
                  
                }

                else
                {
                    DataGridViewRow dr = dataGridView3.SelectedRows[0];
                    this.Hide();
                    frmBookReturn frm = new frmBookReturn();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.txtAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtSubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[9].Value.ToString();
                    frm.AdmissionNo.Text = dr.Cells[10].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtSection.Text = dr.Cells[12].Value.ToString();
                    frm.lblUser.Text = lblUser.Text;
                    frm.lblUserType.Text = lblUserType.Text;
                    frm.btnDelete.Enabled = false;
                    frm.btnUpdate_record.Enabled = false;

                    frm.btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView4_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Set1.Text == "Book issue")
                {
                    DataGridViewRow dr = dataGridView4.SelectedRows[0];
                    this.Hide();
                    frmStudentBookIssue frm = new frmStudentBookIssue();
                    frm.Show();
                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.cmbAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtsubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[10].Value.ToString();
                    frm.cmbadmissionno.Text = dr.Cells[9].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtsection.Text = dr.Cells[12].Value.ToString();
                   
                        frm.btnDelete.Enabled = true;
                        frm.btnUpdate_record.Enabled = true;
                        frm.lblUser.Text = lblUser.Text;
                        frm.lblUserType.Text = lblUserType.Text;

                        frm.btnSave.Enabled = false;
                  
                }

                else
                {
                    DataGridViewRow dr = dataGridView4.SelectedRows[0];
                    this.Hide();
                    frmBookReturn frm = new frmBookReturn();
                    frm.Show();

                    frm.TabControl1.SelectedTab = frm.TabPage1;
                    frm.txtTransactionID.Text = dr.Cells[0].Value.ToString();
                    frm.dtpIssueDate.Text = dr.Cells[1].Value.ToString();

                    frm.dtpDueDate.Text = dr.Cells[2].Value.ToString();
                    frm.txtAccessionNo.Text = dr.Cells[3].Value.ToString();
                    frm.txtBookTitle.Text = dr.Cells[4].Value.ToString();
                    frm.txtAuthor.Text = dr.Cells[5].Value.ToString();
                    frm.txtSubject.Text = dr.Cells[6].Value.ToString();
                    frm.txtISBN.Text = dr.Cells[7].Value.ToString();

                    frm.txtEdition.Text = dr.Cells[8].Value.ToString();
                    frm.txtStudentName.Text = dr.Cells[9].Value.ToString();
                    frm.AdmissionNo.Text = dr.Cells[10].Value.ToString();
                    frm.txtClass.Text = dr.Cells[11].Value.ToString();
                    frm.txtSection.Text = dr.Cells[12].Value.ToString();
                    frm.lblUser.Text = lblUser.Text;
                    frm.lblUserType.Text = lblUserType.Text;
                    frm.btnDelete.Enabled = false;
                    frm.btnUpdate_record.Enabled = false;

                    frm.btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            {
                string strRowNumber = (e.RowIndex + 1).ToString();
                SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
                if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
                {
                    dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
                }
                Brush b = SystemBrushes.ControlText;
                e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            {
                string strRowNumber = (e.RowIndex + 1).ToString();
                SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
                if (dataGridView2.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
                {
                    dataGridView2.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
                }
                Brush b = SystemBrushes.ControlText;
                e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
        }

        private void dataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            {
                string strRowNumber = (e.RowIndex + 1).ToString();
                SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
                if (dataGridView3.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
                {
                    dataGridView3.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
                }
                Brush b = SystemBrushes.ControlText;
                e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
        }

        private void dataGridView4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            {
                string strRowNumber = (e.RowIndex + 1).ToString();
                SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
                if (dataGridView4.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
                {
                    dataGridView4.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
                }
                Brush b = SystemBrushes.ControlText;
                e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Date_from.Text = System.DateTime.Today.ToString();
            Date_to.Text = System.DateTime.Today.ToString();
            dataGridView1.DataSource = null;
            txtStudent.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
            dataGridView2.DataSource = null;
            AdmissionNo.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            IssueDateFrom.Text = System.DateTime.Today.ToString();
            issueDateTo.Text = System.DateTime.Today.ToString();
            dataGridView3.DataSource = null;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DateFrom.Text = System.DateTime.Today.ToString();
            DateTo.Text = System.DateTime.Today.ToString();
            dataGridView4.DataSource = null;
        }

        private void ExportExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = dataGridView1.RowCount - 1;
                colsTotal = dataGridView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView1.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView1.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = dataGridView2.RowCount - 1;
                colsTotal = dataGridView2.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView2.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView2.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView3.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = dataGridView3.RowCount - 1;
                colsTotal = dataGridView3.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView3.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView3.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView4.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = dataGridView4.RowCount - 1;
                colsTotal = dataGridView4.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView4.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView4.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

       
    }
}
