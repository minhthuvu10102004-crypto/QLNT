namespace QLNhaThuoc
{
    partial class chitietphieudoitra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblheader = new DevExpress.XtraEditors.LabelControl();
            flpheader = new System.Windows.Forms.FlowLayoutPanel();
            btnsave = new System.Windows.Forms.Button();
            btnprint = new System.Windows.Forms.Button();
            btnhuy = new System.Windows.Forms.Button();
            pnlnhaplieu = new DevExpress.XtraEditors.PanelControl();
            tlpnhaplieu = new System.Windows.Forms.TableLayoutPanel();
            lbllydo = new DevExpress.XtraEditors.LabelControl();
            luetenncc = new DevExpress.XtraEditors.LookUpEdit();
            lbtdate = new DevExpress.XtraEditors.LabelControl();
            date = new DevExpress.XtraEditors.DateEdit();
            lbltenncc = new DevExpress.XtraEditors.LabelControl();
            lblnv = new DevExpress.XtraEditors.LabelControl();
            luenv = new DevExpress.XtraEditors.LookUpEdit();
            lbltongtien = new DevExpress.XtraEditors.LabelControl();
            caltongtien = new DevExpress.XtraEditors.CalcEdit();
            txtlydo = new DevExpress.XtraEditors.TextEdit();
            applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu(components);
            gridControlnhaplieu = new DevExpress.XtraGrid.GridControl();
            gridViewnhaplieu = new DevExpress.XtraGrid.Views.Grid.GridView();
            lblmaphieu = new DevExpress.XtraEditors.LabelControl();
            flpheader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlnhaplieu).BeginInit();
            pnlnhaplieu.SuspendLayout();
            tlpnhaplieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)luetenncc.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)date.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)date.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)luenv.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)caltongtien.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtlydo.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)applicationMenu1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControlnhaplieu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewnhaplieu).BeginInit();
            SuspendLayout();
            // 
            // lblheader
            // 
            lblheader.Appearance.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblheader.Appearance.ForeColor = System.Drawing.Color.Black;
            lblheader.Appearance.Options.UseFont = true;
            lblheader.Appearance.Options.UseForeColor = true;
            lblheader.Location = new System.Drawing.Point(29, 26);
            lblheader.Name = "lblheader";
            lblheader.Size = new System.Drawing.Size(367, 33);
            lblheader.TabIndex = 1;
            lblheader.Text = "CHI TIẾT PHIẾU TRẢ HÀNG";
            // 
            // flpheader
            // 
            flpheader.Controls.Add(btnsave);
            flpheader.Controls.Add(btnprint);
            flpheader.Controls.Add(btnhuy);
            flpheader.Location = new System.Drawing.Point(760, 12);
            flpheader.Name = "flpheader";
            flpheader.Size = new System.Drawing.Size(432, 52);
            flpheader.TabIndex = 4;
            // 
            // btnsave
            // 
            btnsave.BackColor = System.Drawing.Color.FromArgb(66, 144, 242);
            btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnsave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnsave.ForeColor = System.Drawing.Color.White;
            btnsave.Location = new System.Drawing.Point(3, 3);
            btnsave.Name = "btnsave";
            btnsave.Size = new System.Drawing.Size(114, 40);
            btnsave.TabIndex = 0;
            btnsave.Text = "Lưu";
            btnsave.UseVisualStyleBackColor = false;
            btnsave.Click += btnsave_Click;
            // 
            // btnprint
            // 
            btnprint.BackColor = System.Drawing.Color.FromArgb(66, 144, 242);
            btnprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnprint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnprint.ForeColor = System.Drawing.Color.White;
            btnprint.Location = new System.Drawing.Point(123, 3);
            btnprint.Name = "btnprint";
            btnprint.Size = new System.Drawing.Size(114, 40);
            btnprint.TabIndex = 1;
            btnprint.Text = "In";
            btnprint.UseVisualStyleBackColor = false;
            btnprint.Click += btnprint_Click;
            // 
            // btnhuy
            // 
            btnhuy.BackColor = System.Drawing.Color.FromArgb(66, 144, 242);
            btnhuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnhuy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnhuy.ForeColor = System.Drawing.Color.White;
            btnhuy.Location = new System.Drawing.Point(243, 3);
            btnhuy.Name = "btnhuy";
            btnhuy.Size = new System.Drawing.Size(114, 40);
            btnhuy.TabIndex = 2;
            btnhuy.Text = "Hủy";
            btnhuy.UseVisualStyleBackColor = false;
            btnhuy.Click += btnhuy_Click;
            // 
            // pnlnhaplieu
            // 
            pnlnhaplieu.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pnlnhaplieu.Controls.Add(tlpnhaplieu);
            pnlnhaplieu.Location = new System.Drawing.Point(29, 86);
            pnlnhaplieu.Name = "pnlnhaplieu";
            pnlnhaplieu.Size = new System.Drawing.Size(1121, 254);
            pnlnhaplieu.TabIndex = 5;
            // 
            // tlpnhaplieu
            // 
            tlpnhaplieu.ColumnCount = 2;
            tlpnhaplieu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.6297951F));
            tlpnhaplieu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.37021F));
            tlpnhaplieu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tlpnhaplieu.Controls.Add(lbllydo, 0, 3);
            tlpnhaplieu.Controls.Add(luetenncc, 1, 1);
            tlpnhaplieu.Controls.Add(lbtdate, 0, 0);
            tlpnhaplieu.Controls.Add(date, 1, 0);
            tlpnhaplieu.Controls.Add(lbltenncc, 0, 1);
            tlpnhaplieu.Controls.Add(lblnv, 0, 2);
            tlpnhaplieu.Controls.Add(luenv, 1, 2);
            tlpnhaplieu.Controls.Add(lbltongtien, 0, 4);
            tlpnhaplieu.Controls.Add(caltongtien, 1, 4);
            tlpnhaplieu.Controls.Add(txtlydo, 1, 3);
            tlpnhaplieu.Location = new System.Drawing.Point(0, 0);
            tlpnhaplieu.Name = "tlpnhaplieu";
            tlpnhaplieu.RowCount = 5;
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tlpnhaplieu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tlpnhaplieu.Size = new System.Drawing.Size(1121, 254);
            tlpnhaplieu.TabIndex = 0;
            // 
            // lbllydo
            // 
            lbllydo.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbllydo.Appearance.Options.UseFont = true;
            lbllydo.Location = new System.Drawing.Point(3, 153);
            lbllydo.Name = "lbllydo";
            lbllydo.Size = new System.Drawing.Size(83, 21);
            lbllydo.TabIndex = 21;
            lbllydo.Text = "Lý do trả:";
            // 
            // luetenncc
            // 
            luetenncc.Location = new System.Drawing.Point(167, 53);
            luetenncc.Name = "luetenncc";
            luetenncc.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            luetenncc.Properties.Appearance.Options.UseFont = true;
            luetenncc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            luetenncc.Size = new System.Drawing.Size(548, 28);
            luetenncc.TabIndex = 20;
            // 
            // lbtdate
            // 
            lbtdate.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbtdate.Appearance.Options.UseFont = true;
            lbtdate.Location = new System.Drawing.Point(3, 3);
            lbtdate.Name = "lbtdate";
            lbtdate.Size = new System.Drawing.Size(83, 21);
            lbtdate.TabIndex = 6;
            lbtdate.Text = "Ngày lập:";
            // 
            // date
            // 
            date.EditValue = null;
            date.Location = new System.Drawing.Point(167, 3);
            date.Name = "date";
            date.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            date.Properties.Appearance.Options.UseFont = true;
            date.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            date.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            date.Size = new System.Drawing.Size(308, 28);
            date.TabIndex = 7;
            // 
            // lbltenncc
            // 
            lbltenncc.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltenncc.Appearance.Options.UseFont = true;
            lbltenncc.Location = new System.Drawing.Point(3, 53);
            lbltenncc.Name = "lbltenncc";
            lbltenncc.Size = new System.Drawing.Size(125, 21);
            lbltenncc.TabIndex = 14;
            lbltenncc.Text = "Nhà cung cấp:";
            // 
            // lblnv
            // 
            lblnv.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblnv.Appearance.Options.UseFont = true;
            lblnv.Location = new System.Drawing.Point(3, 103);
            lblnv.Name = "lblnv";
            lblnv.Size = new System.Drawing.Size(93, 21);
            lblnv.TabIndex = 12;
            lblnv.Text = "Người lập:";
            // 
            // luenv
            // 
            luenv.Location = new System.Drawing.Point(167, 103);
            luenv.Name = "luenv";
            luenv.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            luenv.Properties.Appearance.Options.UseFont = true;
            luenv.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            luenv.Size = new System.Drawing.Size(268, 28);
            luenv.TabIndex = 13;
            // 
            // lbltongtien
            // 
            lbltongtien.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltongtien.Appearance.Options.UseFont = true;
            lbltongtien.Location = new System.Drawing.Point(3, 203);
            lbltongtien.Name = "lbltongtien";
            lbltongtien.Size = new System.Drawing.Size(89, 21);
            lbltongtien.TabIndex = 18;
            lbltongtien.Text = "Tổng tiền:";
            // 
            // caltongtien
            // 
            caltongtien.Location = new System.Drawing.Point(167, 203);
            caltongtien.Name = "caltongtien";
            caltongtien.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            caltongtien.Properties.Appearance.Options.UseFont = true;
            caltongtien.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            caltongtien.Size = new System.Drawing.Size(163, 28);
            caltongtien.TabIndex = 19;
            // 
            // txtlydo
            // 
            txtlydo.Location = new System.Drawing.Point(167, 153);
            txtlydo.Name = "txtlydo";
            txtlydo.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txtlydo.Properties.Appearance.Options.UseFont = true;
            txtlydo.Size = new System.Drawing.Size(199, 28);
            txtlydo.TabIndex = 22;
            // 
            // applicationMenu1
            // 
            applicationMenu1.Name = "applicationMenu1";
            // 
            // gridControlnhaplieu
            // 
            gridControlnhaplieu.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridControlnhaplieu.Location = new System.Drawing.Point(29, 346);
            gridControlnhaplieu.MainView = gridViewnhaplieu;
            gridControlnhaplieu.Name = "gridControlnhaplieu";
            gridControlnhaplieu.Size = new System.Drawing.Size(1121, 290);
            gridControlnhaplieu.TabIndex = 6;
            gridControlnhaplieu.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewnhaplieu });
            // 
            // gridViewnhaplieu
            // 
            gridViewnhaplieu.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            gridViewnhaplieu.Appearance.HeaderPanel.Options.UseFont = true;
            gridViewnhaplieu.Appearance.Row.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridViewnhaplieu.Appearance.Row.Options.UseFont = true;
            gridViewnhaplieu.GridControl = gridControlnhaplieu;
            gridViewnhaplieu.Name = "gridViewnhaplieu";
            gridViewnhaplieu.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            gridViewnhaplieu.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            gridViewnhaplieu.OptionsView.ColumnAutoWidth = false;
            gridViewnhaplieu.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            gridViewnhaplieu.OptionsView.ShowGroupPanel = false;
            // 
            // lblmaphieu
            // 
            lblmaphieu.Appearance.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblmaphieu.Appearance.ForeColor = System.Drawing.Color.Black;
            lblmaphieu.Appearance.Options.UseFont = true;
            lblmaphieu.Appearance.Options.UseForeColor = true;
            lblmaphieu.Location = new System.Drawing.Point(438, 26);
            lblmaphieu.Name = "lblmaphieu";
            lblmaphieu.Size = new System.Drawing.Size(40, 33);
            lblmaphieu.TabIndex = 7;
            lblmaphieu.Text = ".....";
            lblmaphieu.Visible = false;
            // 
            // chitietphieudoitra
            // 
            Appearance.BackColor = System.Drawing.Color.FromArgb(246, 247, 249);
            Appearance.Options.UseBackColor = true;
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1189, 648);
            Controls.Add(lblmaphieu);
            Controls.Add(gridControlnhaplieu);
            Controls.Add(pnlnhaplieu);
            Controls.Add(flpheader);
            Controls.Add(lblheader);
            Name = "chitietphieudoitra";
            Text = "Chi tiết phiếu trả";
            Load += chitietphieudoitra_Load;
            flpheader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlnhaplieu).EndInit();
            pnlnhaplieu.ResumeLayout(false);
            tlpnhaplieu.ResumeLayout(false);
            tlpnhaplieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)luetenncc.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)date.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)date.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)luenv.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)caltongtien.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtlydo.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)applicationMenu1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControlnhaplieu).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewnhaplieu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblheader;
        private System.Windows.Forms.FlowLayoutPanel flpheader;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnprint;
        private System.Windows.Forms.Button btnhuy;
        private DevExpress.XtraEditors.PanelControl pnlnhaplieu;
        private System.Windows.Forms.TableLayoutPanel tlpnhaplieu;
        private DevExpress.XtraEditors.LabelControl lbtdate;
        private DevExpress.XtraEditors.DateEdit date;
        private DevExpress.XtraEditors.LabelControl lblNCC;
        
        private DevExpress.XtraEditors.LookUpEdit luesophieu;
        
        private DevExpress.XtraEditors.TextEdit txtspn;
        private DevExpress.XtraEditors.LabelControl lbltenncc;
        private DevExpress.XtraEditors.LookUpEdit luenv;
        private DevExpress.XtraEditors.LabelControl lblnv;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.XtraGrid.GridControl gridControlnhaplieu;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewnhaplieu;
        private DevExpress.XtraEditors.LabelControl lbltongtien;
        private DevExpress.XtraEditors.CalcEdit caltongtien;
        private DevExpress.XtraEditors.LookUpEdit luepttt;
        private DevExpress.XtraEditors.LabelControl lblpttt;
        private DevExpress.XtraEditors.LabelControl lbtspn;
        private DevExpress.XtraEditors.TextEdit txtshipper;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblmaphieu;
        private DevExpress.XtraEditors.LookUpEdit luetenncc;
        private DevExpress.XtraEditors.LabelControl lbllydo;
        private DevExpress.XtraEditors.TextEdit txtlydo;
    }
}