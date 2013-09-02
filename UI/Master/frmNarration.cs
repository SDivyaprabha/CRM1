using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.Utils.Paint;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmNarration : DevExpress.XtraEditors.XtraForm
    {
        #region Contructor

        public frmNarration()
        {
            InitializeComponent();
        }

        #endregion

        #region FormEvents

        private void frmNarration_Load(object sender, EventArgs e)
        {
            SetMyGraphics();
            if (BsfGlobal.FindPermission("Narration-Master-Add") == false)
            {
                MessageBox.Show("No Rights to Add Narration", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdNarrationView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            }
            else
                grdNarrationView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;

            if (BsfGlobal.FindPermission("Narration-Master-Edit") == false)
            {
                MessageBox.Show("No Rights to Edit Narration", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdNarrationView.OptionsBehavior.Editable = false;
            }
            else
                grdNarrationView.OptionsBehavior.Editable = true;

            if (BsfGlobal.FindPermission("Narration-Master-Delete") == false)
            {
                MessageBox.Show("No Rights to Delete Narration", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
                btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;


            PopulateGrid();
        }

        private void frmNarration_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { Parent.Controls.Owner.Hide(); }
                    catch { }
            }
        }

        #endregion

        #region Functions

        private static void SetMyGraphics()
        {
            FieldInfo fi = typeof(XPaint).GetField("graphics", BindingFlags.Static | BindingFlags.NonPublic);
            fi.SetValue(null, new MyXPaint());
        }

        public class MyXPaint : XPaint
        {
            public override void DrawFocusRectangle(Graphics g, Rectangle r, Color foreColor, Color backColor)
            {
                if (!CanDraw(r)) return;
                Brush hb = Brushes.Red;
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, 2, r.Height - 2)); // left
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 2, 2)); // top
                g.FillRectangle(hb, new Rectangle(r.Right - 2, r.Y, 2, r.Height - 2)); // right
                g.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 2, r.Width, 2)); // bottom
            }
        }

        private void PopulateGrid()
        {
            DataTable dtNarration = new DataTable();
            dtNarration = ReceiptDetailBL.PopulateNarrationMaster();
            grdNarration.DataSource = dtNarration;
            grdNarrationView.PopulateColumns();
            grdNarrationView.Columns["NarrationId"].Visible = false;
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit txtTerms = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            grdNarrationView.Columns["Description"].ColumnEdit = txtTerms;
            grdNarrationView.OptionsView.RowAutoHeight = true;
            grdNarrationView.Appearance.HeaderPanel.Font = new Font(grdNarrationView.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        #endregion

        #region GridEvents

        private void grdNarrationView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int PRow = grdNarrationView.FocusedRowHandle;
            int NarrId = Convert.ToInt32(CommFun.IsNullCheck(grdNarrationView.GetFocusedRowCellValue("NarrationId"), CommFun.datatypes.vartypenumeric));
            string NarrDescription = Convert.ToString(CommFun.IsNullCheck(grdNarrationView.GetFocusedRowCellValue("Description"), CommFun.datatypes.vartypestring));
            ReceiptDetailBL.InsertNarrationMaster(NarrId, NarrDescription);
            grdNarrationView.FocusedRowHandle = PRow;
        }

        private void grdNarrationView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();

            }
        }

        #endregion

        #region ButtonEvents

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdNarrationView.FocusedRowHandle = grdNarrationView.FocusedRowHandle + 1;
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult reply = MessageBox.Show("Do you want to delete this particular Narration ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                if (grdNarrationView.IsNewItemRow(grdNarrationView.FocusedRowHandle) == true)
                    grdNarrationView.DeleteRow(grdNarrationView.FocusedRowHandle);
                else
                {
                    int NarrId = Convert.ToInt32(CommFun.IsNullCheck(grdNarrationView.GetFocusedRowCellValue("NarrationId").ToString(), CommFun.datatypes.vartypestring));
                    ReceiptDetailBL.DeleteNarrationMaster(NarrId);
                    grdNarrationView.DeleteRow(grdNarrationView.FocusedRowHandle);
                }

            }
        }
        #endregion
    }
}
