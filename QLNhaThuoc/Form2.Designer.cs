namespace QLNhaThuoc
{
    partial class frmHoaDonBH
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblSDT = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblten = new System.Windows.Forms.Label();
            lblTenKH = new System.Windows.Forms.Label();
            lbldt = new System.Windows.Forms.Label();
            sdt = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            dgvChiTietHD = new System.Windows.Forms.DataGridView();
            STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            SoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            lblMaHoaDon = new System.Windows.Forms.ToolStripStatusLabel();
            tsslMaHD = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            tsslNgayLap = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            tsslNhanVien = new System.Windows.Forms.ToolStripStatusLabel();
            timer1 = new System.Windows.Forms.Timer(components);
            lblPTTT = new System.Windows.Forms.Label();
            PTTT = new System.Windows.Forms.Label();
            lblTongTien = new System.Windows.Forms.Label();
            Tong = new System.Windows.Forms.Label();
            diachi = new System.Windows.Forms.Label();
            lbldiachi = new System.Windows.Forms.Label();
            lblemail = new System.Windows.Forms.Label();
            email = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietHD).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(306, 206);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(343, 37);
            label1.TabIndex = 0;
            label1.Text = "HÓA ĐƠN BÁN HÀNG";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(209, 41);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(217, 33);
            label2.TabIndex = 1;
            label2.Text = "Nhà thuốc ABC";
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblSDT.Location = new System.Drawing.Point(211, 89);
            lblSDT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new System.Drawing.Size(298, 23);
            lblSDT.TabIndex = 3;
            lblSDT.Text = "SDT: 0912345678 - 0987654321";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(211, 125);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(488, 23);
            label3.TabIndex = 4;
            label3.Text = "Địa chỉ: 124 Giải Phóng, phường Hai Bà Trưng, Hà Nội";
            // 
            // lblten
            // 
            lblten.AutoSize = true;
            lblten.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblten.Location = new System.Drawing.Point(72, 284);
            lblten.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblten.Name = "lblten";
            lblten.Size = new System.Drawing.Size(172, 24);
            lblten.TabIndex = 5;
            lblten.Text = "Tên Khách hàng:";
            // 
            // lblTenKH
            // 
            lblTenKH.AutoSize = true;
            lblTenKH.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblTenKH.Location = new System.Drawing.Point(252, 284);
            lblTenKH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new System.Drawing.Size(103, 23);
            lblTenKH.TabIndex = 6;
            lblTenKH.Text = "Nguyễn An";
            // 
            // lbldt
            // 
            lbldt.AutoSize = true;
            lbldt.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbldt.Location = new System.Drawing.Point(523, 283);
            lbldt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbldt.Name = "lbldt";
            lbldt.Size = new System.Drawing.Size(141, 24);
            lbldt.TabIndex = 7;
            lbldt.Text = "Số điện thoại:";
            // 
            // sdt
            // 
            sdt.AutoSize = true;
            sdt.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            sdt.Location = new System.Drawing.Point(672, 283);
            sdt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            sdt.Name = "sdt";
            sdt.Size = new System.Drawing.Size(120, 23);
            sdt.TabIndex = 8;
            sdt.Text = "0912345679";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(78, 19);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(124, 155);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // dgvChiTietHD
            // 
            dgvChiTietHD.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(66, 144, 242);
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvChiTietHD.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvChiTietHD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTietHD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { STT, TenThuoc, SoLuong, DonGia, ThanhTien });
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(66, 144, 242);
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvChiTietHD.DefaultCellStyle = dataGridViewCellStyle4;
            dgvChiTietHD.Location = new System.Drawing.Point(72, 380);
            dgvChiTietHD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            dgvChiTietHD.Name = "dgvChiTietHD";
            dgvChiTietHD.RowHeadersVisible = false;
            dgvChiTietHD.RowHeadersWidth = 51;
            dgvChiTietHD.RowTemplate.Height = 24;
            dgvChiTietHD.Size = new System.Drawing.Size(818, 440);
            dgvChiTietHD.TabIndex = 9;
            // 
            // STT
            // 
            STT.HeaderText = "STT";
            STT.MinimumWidth = 6;
            STT.Name = "STT";
            STT.Width = 60;
            // 
            // TenThuoc
            // 
            TenThuoc.HeaderText = "Danh sách thuốc";
            TenThuoc.MinimumWidth = 6;
            TenThuoc.Name = "TenThuoc";
            TenThuoc.Width = 300;
            // 
            // SoLuong
            // 
            SoLuong.HeaderText = "Số lượng";
            SoLuong.MinimumWidth = 6;
            SoLuong.Name = "SoLuong";
            SoLuong.Width = 140;
            // 
            // DonGia
            // 
            DonGia.HeaderText = "Đơn giá";
            DonGia.MinimumWidth = 6;
            DonGia.Name = "DonGia";
            DonGia.ReadOnly = true;
            DonGia.Width = 140;
            // 
            // ThanhTien
            // 
            ThanhTien.HeaderText = "Thành tiền";
            ThanhTien.MinimumWidth = 6;
            ThanhTien.Name = "ThanhTien";
            ThanhTien.ReadOnly = true;
            ThanhTien.Width = 175;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblMaHoaDon, tsslMaHD, toolStripStatusLabel2, toolStripStatusLabel3, tsslNgayLap, toolStripStatusLabel1, toolStripStatusLabel5, tsslNhanVien });
            statusStrip1.Location = new System.Drawing.Point(0, 839);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            statusStrip1.Size = new System.Drawing.Size(978, 32);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblMaHoaDon.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new System.Drawing.Size(112, 25);
            lblMaHoaDon.Text = "Mã hóa đơn:";
            // 
            // tsslMaHD
            // 
            tsslMaHD.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            tsslMaHD.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsslMaHD.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            tsslMaHD.Name = "tsslMaHD";
            tsslMaHD.Size = new System.Drawing.Size(76, 25);
            tsslMaHD.Text = "HD0001";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(16, 25);
            toolStripStatusLabel2.Text = "|";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toolStripStatusLabel3.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new System.Drawing.Size(158, 25);
            toolStripStatusLabel3.Text = "Ngày lập hóa đơn:";
            // 
            // tsslNgayLap
            // 
            tsslNgayLap.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsslNgayLap.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            tsslNgayLap.Name = "tsslNgayLap";
            tsslNgayLap.Size = new System.Drawing.Size(150, 25);
            tsslNgayLap.Text = "10/10/2025 21:23";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(16, 25);
            toolStripStatusLabel1.Text = "|";
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toolStripStatusLabel5.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Size = new System.Drawing.Size(176, 25);
            toolStripStatusLabel5.Text = "Nhân viên bán hàng:";
            // 
            // tsslNhanVien
            // 
            tsslNhanVien.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsslNhanVien.LinkColor = System.Drawing.Color.FromArgb(66, 144, 242);
            tsslNhanVien.Name = "tsslNhanVien";
            tsslNhanVien.Size = new System.Drawing.Size(73, 25);
            tsslNhanVien.Text = "An Bình";
            // 
            // lblPTTT
            // 
            lblPTTT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblPTTT.AutoSize = true;
            lblPTTT.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblPTTT.Location = new System.Drawing.Point(719, 764);
            lblPTTT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPTTT.Name = "lblPTTT";
            lblPTTT.Size = new System.Drawing.Size(133, 23);
            lblPTTT.TabIndex = 14;
            lblPTTT.Text = "Chuyển khoản";
            // 
            // PTTT
            // 
            PTTT.AutoSize = true;
            PTTT.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            PTTT.Location = new System.Drawing.Point(459, 764);
            PTTT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PTTT.Name = "PTTT";
            PTTT.Size = new System.Drawing.Size(252, 24);
            PTTT.TabIndex = 13;
            PTTT.Text = "Phương thức thanh toán:";
            // 
            // lblTongTien
            // 
            lblTongTien.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblTongTien.Location = new System.Drawing.Point(719, 717);
            lblTongTien.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new System.Drawing.Size(127, 23);
            lblTongTien.TabIndex = 12;
            lblTongTien.Text = "00,000,000 đ";
            // 
            // Tong
            // 
            Tong.AutoSize = true;
            Tong.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Tong.Location = new System.Drawing.Point(647, 712);
            Tong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Tong.Name = "Tong";
            Tong.Size = new System.Drawing.Size(64, 28);
            Tong.TabIndex = 11;
            Tong.Text = "Tổng:";
            // 
            // diachi
            // 
            diachi.AutoSize = true;
            diachi.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            diachi.Location = new System.Drawing.Point(161, 334);
            diachi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            diachi.Name = "diachi";
            diachi.Size = new System.Drawing.Size(72, 23);
            diachi.TabIndex = 16;
            diachi.Text = "[Trống]";
            // 
            // lbldiachi
            // 
            lbldiachi.AutoSize = true;
            lbldiachi.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbldiachi.Location = new System.Drawing.Point(72, 334);
            lbldiachi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbldiachi.Name = "lbldiachi";
            lbldiachi.Size = new System.Drawing.Size(81, 24);
            lbldiachi.TabIndex = 15;
            lbldiachi.Text = "Địa chỉ:";
            // 
            // lblemail
            // 
            lblemail.AutoSize = true;
            lblemail.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblemail.Location = new System.Drawing.Point(523, 334);
            lblemail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblemail.Name = "lblemail";
            lblemail.Size = new System.Drawing.Size(68, 24);
            lblemail.TabIndex = 17;
            lblemail.Text = "Email:";
            // 
            // email
            // 
            email.AutoSize = true;
            email.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            email.Location = new System.Drawing.Point(599, 334);
            email.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            email.Name = "email";
            email.Size = new System.Drawing.Size(72, 23);
            email.TabIndex = 18;
            email.Text = "[Trống]";
            // 
            // frmHoaDonBH
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(978, 871);
            Controls.Add(email);
            Controls.Add(lblemail);
            Controls.Add(diachi);
            Controls.Add(lbldiachi);
            Controls.Add(lblPTTT);
            Controls.Add(PTTT);
            Controls.Add(lblTongTien);
            Controls.Add(Tong);
            Controls.Add(statusStrip1);
            Controls.Add(dgvChiTietHD);
            Controls.Add(sdt);
            Controls.Add(lbldt);
            Controls.Add(lblTenKH);
            Controls.Add(lblten);
            Controls.Add(label3);
            Controls.Add(lblSDT);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "frmHoaDonBH";
            Text = "Form2";
            Load += frmHoaDonBH_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietHD).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblten;
        private System.Windows.Forms.Label lblTenKH;
        private System.Windows.Forms.Label lbldt;
        private System.Windows.Forms.Label sdt;
        private System.Windows.Forms.DataGridView dgvChiTietHD;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMaHoaDon;
        private System.Windows.Forms.ToolStripStatusLabel tsslMaHD;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsslNgayLap;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblPTTT;
        private System.Windows.Forms.Label PTTT;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label Tong;
        private System.Windows.Forms.Label diachi;
        private System.Windows.Forms.Label lbldiachi;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel tsslNhanVien;
        private System.Windows.Forms.Label lblemail;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThanhTien;
    }
}