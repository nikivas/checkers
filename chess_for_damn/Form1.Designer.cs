namespace chess_for_damn
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.white = new System.Windows.Forms.PictureBox();
            this.black = new System.Windows.Forms.PictureBox();
            this.black_chess = new System.Windows.Forms.PictureBox();
            this.white_chess = new System.Windows.Forms.PictureBox();
            this.possible_step = new System.Windows.Forms.PictureBox();
            this.white_queen = new System.Windows.Forms.PictureBox();
            this.black_queen = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.white)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.black)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.black_chess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.white_chess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.possible_step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.white_queen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.black_queen)).BeginInit();
            this.SuspendLayout();
            // 
            // white
            // 
            this.white.Image = ((System.Drawing.Image)(resources.GetObject("white.Image")));
            this.white.Location = new System.Drawing.Point(66, 70);
            this.white.Name = "white";
            this.white.Size = new System.Drawing.Size(57, 50);
            this.white.TabIndex = 0;
            this.white.TabStop = false;
            this.white.Visible = false;
            this.white.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // black
            // 
            this.black.Image = ((System.Drawing.Image)(resources.GetObject("black.Image")));
            this.black.Location = new System.Drawing.Point(142, 70);
            this.black.Name = "black";
            this.black.Size = new System.Drawing.Size(56, 50);
            this.black.TabIndex = 1;
            this.black.TabStop = false;
            this.black.Visible = false;
            // 
            // black_chess
            // 
            this.black_chess.Image = ((System.Drawing.Image)(resources.GetObject("black_chess.Image")));
            this.black_chess.Location = new System.Drawing.Point(223, 50);
            this.black_chess.Name = "black_chess";
            this.black_chess.Size = new System.Drawing.Size(71, 71);
            this.black_chess.TabIndex = 2;
            this.black_chess.TabStop = false;
            this.black_chess.Visible = false;
            // 
            // white_chess
            // 
            this.white_chess.Image = ((System.Drawing.Image)(resources.GetObject("white_chess.Image")));
            this.white_chess.Location = new System.Drawing.Point(332, 50);
            this.white_chess.Name = "white_chess";
            this.white_chess.Size = new System.Drawing.Size(71, 71);
            this.white_chess.TabIndex = 3;
            this.white_chess.TabStop = false;
            this.white_chess.Visible = false;
            // 
            // possible_step
            // 
            this.possible_step.Image = ((System.Drawing.Image)(resources.GetObject("possible_step.Image")));
            this.possible_step.Location = new System.Drawing.Point(435, 50);
            this.possible_step.Name = "possible_step";
            this.possible_step.Size = new System.Drawing.Size(71, 71);
            this.possible_step.TabIndex = 4;
            this.possible_step.TabStop = false;
            this.possible_step.Visible = false;
            // 
            // white_queen
            // 
            this.white_queen.Image = ((System.Drawing.Image)(resources.GetObject("white_queen.Image")));
            this.white_queen.Location = new System.Drawing.Point(66, 157);
            this.white_queen.Name = "white_queen";
            this.white_queen.Size = new System.Drawing.Size(72, 69);
            this.white_queen.TabIndex = 5;
            this.white_queen.TabStop = false;
            this.white_queen.Visible = false;
            // 
            // black_queen
            // 
            this.black_queen.Image = ((System.Drawing.Image)(resources.GetObject("black_queen.Image")));
            this.black_queen.Location = new System.Drawing.Point(223, 157);
            this.black_queen.Name = "black_queen";
            this.black_queen.Size = new System.Drawing.Size(70, 70);
            this.black_queen.TabIndex = 6;
            this.black_queen.TabStop = false;
            this.black_queen.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(660, 50);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(188, 467);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(660, 539);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 26);
            this.button1.TabIndex = 8;
            this.button1.Text = "выход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(770, 539);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 22);
            this.button2.TabIndex = 9;
            this.button2.Text = "новая";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(710, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "__________";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 560);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(537, 39);
            this.label2.TabIndex = 11;
            this.label2.Text = "a      b      c      d      e      f      g      h";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(560, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 555);
            this.label3.TabIndex = 12;
            this.label3.Text = "1\r\n\r\n2\r\n\r\n3\r\n\r\n4\r\n\r\n5\r\n\r\n6\r\n\r\n7\r\n\r\n8";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 605);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.black_queen);
            this.Controls.Add(this.white_queen);
            this.Controls.Add(this.possible_step);
            this.Controls.Add(this.white_chess);
            this.Controls.Add(this.black_chess);
            this.Controls.Add(this.black);
            this.Controls.Add(this.white);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Шашки";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.white)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.black)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.black_chess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.white_chess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.possible_step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.white_queen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.black_queen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox white;
        private System.Windows.Forms.PictureBox black;
        private System.Windows.Forms.PictureBox black_chess;
        private System.Windows.Forms.PictureBox white_chess;
        private System.Windows.Forms.PictureBox possible_step;
        private System.Windows.Forms.PictureBox white_queen;
        private System.Windows.Forms.PictureBox black_queen;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Timer timer1;
    }
}

