using DevExpress.LookAndFeel;
using DevExpress.Map.Native;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using QLNhaThuoc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls.Ribbon;
using System.Windows.Forms;

namespace QLNhaThuoc
{
    public partial class frmmain : DevExpress.XtraEditors.XtraForm
    {
        public frmmain()
        {
            InitializeComponent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            // Giãn đều giữa các BarButtonItem
            // Thiết lập mặc định
            barStaticItemstt.Caption = "Ready";
            barStaticItemTime.Caption = DateTime.Now.ToString("HH:mm:ss");

            // Tạo timer cập nhật thời gian
            Timer timer = new Timer();
            timer.Interval = 1000; // 1 giây
            timer.Tick += (s, e) =>
            {
                barStaticItemTime.Caption = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            };
            timer.Start();


        }

        private void frmmain_Load(object sender, EventArgs e)
        {

            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinMaskColor = Color.FromArgb(12, 154, 255);

        }

        private void btnthoat_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnnv_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            nhanvien nhanvien = new nhanvien() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };

            pnlform.Controls.Add(nhanvien);
            nhanvien.Show();

        }

        private void btnncc_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            nhacungcap nhacungcap = new nhacungcap() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };

            pnlform.Controls.Add(nhacungcap);
            nhacungcap.Show();
        }

        private void btnnhaphang_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            nhaphang nhaphang = new nhaphang() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            pnlform.Controls.Add(nhaphang);
            nhaphang.Show();
        }

        private void btnbctk_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            FormInventoryReport f = new FormInventoryReport() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            f.Font = new Font("Arial", 9, FontStyle.Regular);
            pnlform.Controls.Add(f);
            f.Show();
        }

        private void btnbcdt_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            FormRevenueReport f = new FormRevenueReport() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            f.Font = new Font("Arial", 9, FontStyle.Regular);
            pnlform.Controls.Add(f);
            f.Show();
        }

        private void btnimport_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            FormImportReport f = new FormImportReport() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            f.Font = new Font("Arial", 9, FontStyle.Regular);
            pnlform.Controls.Add(f);
            f.Show();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            FormReturnReport f = new FormReturnReport() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            f.Font = new Font("Arial", 9, FontStyle.Regular);
            pnlform.Controls.Add(f);
            f.Show();

        }

        private void btnthuoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            pnlform.Controls.Clear();
            FormWarehouse f = new FormWarehouse() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            f.Font = new Font("Arial", 9, FontStyle.Regular);
            pnlform.Controls.Add(f);
            f.Show();
        }
    }
}

