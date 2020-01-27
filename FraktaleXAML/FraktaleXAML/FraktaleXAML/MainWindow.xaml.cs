
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Windows.Forms;


namespace FraktaleXAML
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            canvas_draw.UpdateLayout();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick; //event!!
            dispatcherTimer.Interval = new TimeSpan(20, 0, 0);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Start();
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Canvas.SetLeft(arrow_image, Mouse.GetPosition(arrow_canvas).X - arrow_image.Width/2);
            Canvas.SetTop(arrow_image, Mouse.GetPosition(arrow_canvas).Y - arrow_image.Height/2);
        }

        List<Tuple<double, int, int, double>> punkty = new List<Tuple<double, int, int, double>>();
        List<Tuple<double, int, int>> fraki = new List<Tuple<double, int, int>>();
        List<Tuple<double, int, int>> trojkaty = new List<Tuple<double, int, int>>();
        List<Tuple<int, int, int, int, double, double>> Julie = new List<Tuple<int, int, int, int, double, double>>();
        List<Tuple<int, int, int, int>> Mandy = new List<Tuple<int, int, int, int>>();
        List<string> dodane = new List<string>();



        private void Button_Upload(object sender, RoutedEventArgs e)
        {
            
            textBox_punkt.Text = len_in.ToString();
        }

        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            if(dodane.Last() == "drzewo")
            {
                dodane.Remove(dodane.Last());
                fraki.RemoveAt(drz_it-1);
                drz_it -= 1;
            }
            if (dodane.Last() == "trojkat")
            {
                dodane.Remove(dodane.Last());
                trojkaty.RemoveAt(tr_it-1);
                tr_it -= 1;
            }/*
            if (fraki.Count > 0)
            {
                fraki.Remove(fraki.Last());
            }*/
            list_it.Items.RemoveAt(list_it.Items.Count-1);
            canvas_draw.Children.Clear();
            Draw_All();
            
        }

        int x;
        int y;
        int len_in = 40;
        int drz_it = 0;
        int tr_it = 0;
        string last_item = "";

        private void Draw_All() {

            
                foreach (Tuple<double, int, int> frak in fraki)
                {
                    double l1 = frak.Item1;
                    int x1 = frak.Item2;
                    int y1 = frak.Item3;
                    //int angle = int.Parse(kat_txt.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    rysuj_drzewo(l1, x1, y1, 0);
                }

            
            foreach (Tuple<double, int, int> frak in trojkaty)
            {

                double l1 = frak.Item1;
                int x1 = frak.Item2;
                int y1 = frak.Item3;
                //int angle = int.Parse(kat_txt.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                rysuj_trojkat(l1, x1, y1);

            }
            foreach(Tuple<int, int, int, int, double, double> frak in Julie)
            {
                int x1 = frak.Item1;
                int y1 = frak.Item2;
                int sc = frak.Item3;
                int ang = frak.Item4;
                double im = frak.Item5;
                double re = frak.Item6;
                rysuj_Julia(x1, y1, ang, sc, im, re);
            }
            foreach (Tuple<int, int, int, int> frak in Mandy)
            {
                int x1 = frak.Item1;
                int y1 = frak.Item2;
                int sc = frak.Item3;
                int ang = frak.Item4;
                
                rysuj_MandelBrot(x1, y1, ang, sc);
            }

        }

        void rysuj_trojkat(double length, int x, int y)
        {
            Line linia = new Line();
            Line linia2 = new Line();
            Line linia3 = new Line();
            linia.Stroke = System.Windows.Media.Brushes.Black;
            linia2.Stroke = System.Windows.Media.Brushes.Black;
            linia3.Stroke = System.Windows.Media.Brushes.Black;
            canvas_draw.Children.Add(linia);
            canvas_draw.Children.Add(linia2);
            canvas_draw.Children.Add(linia3);

            //dodać ten kąt tu gdzies
            if (length > 5)
            {
                linia.X1 = x - (int)(length / 2);
                linia.Y1 = y + (int)(length * Math.Sqrt(3) / 4);
                linia.X2 = x + (int)(length / 2);
                linia.Y2 = y + (int)(length * Math.Sqrt(3) / 4);

                linia2.X1 = x - (int)(length / 2);
                linia2.Y1 = y + (int)(length * Math.Sqrt(3) / 4);
                linia2.X2 = x;
                linia2.Y2 = y - (int)(length * Math.Sqrt(3) / 4);

                linia3.X1 = x;
                linia3.Y1 = y - (int)(length * Math.Sqrt(3) / 4);
                linia3.X2 = x + (int)(length / 2);
                linia3.Y2 = y + (int)(length * Math.Sqrt(3) / 4);

                rysuj_trojkat(length / 2, x, y - (int)(length * Math.Sqrt(3) / 8));
                rysuj_trojkat(length / 2, x - (int)(length / 4), y + (int)(length * Math.Sqrt(3) / 8));
                rysuj_trojkat(length / 2, x + (int)(length / 4), y + (int)(length * Math.Sqrt(3) / 8));
            }
        }

        void rysuj_drzewo(double length, int xx, int yy, float ang)
        {
            Line linia = new Line();
            linia.Stroke = System.Windows.Media.Brushes.Black;

            //wyliczenie punktów końcowych dla tego stopnia iteracji
            int new_x = xx + (int)(Math.Sin(ang) * length);
            int new_y = yy - (int)(Math.Cos(ang) * length);
            double new_ang = (float)(30 * (Math.PI) / 180) + ang;
            //faktyczne rysowanie
            linia.X1 = xx;
            linia.X2 = new_x;
            linia.Y1 = yy;
            linia.Y2 = new_y;
            canvas_draw.Children.Add(linia);
            
            //nowa długość
            length *= 0.75;
            if (length > 2)
            {
                //dodanie punktu do listy, aby się do niego cofnąć
                punkty.Add(Tuple.Create(length, new_x, new_y, new_ang));
                //rysowanie prawej gałezi
                rysuj_drzewo(length, new_x, new_y, (float)new_ang);
                //cofnięcie i usunięcie
                length = punkty.Last().Item1;
                new_x = punkty.Last().Item2;
                new_y = punkty.Last().Item3;
                new_ang = punkty.Last().Item4;
                punkty.Remove(punkty.Last());
                //rysowanie lewej gałęzi
                rysuj_drzewo(length, new_x, new_y, (float)new_ang - 2 * (float)(30 * (Math.PI) / 180));
            }

        }

        void rysuj_MandelBrot(int xx, int yy, int ann, int sca)
        {

            double xmin, xmax, ymin, ymax = new double();
            xmin = -2;
            xmax = 2;
            ymin = -2;
            ymax = 2;
            ImageBrush img = new ImageBrush();
            List<byte> buffer = new List<byte>();
            int a, b, kmax, m = new int();
            a = 600;
            b = 600;
            kmax = 200;
            m = 2;
            int[,] images = new int[a, b];
            double x0, y0, dx, dy, r = new double();
            // ' Dim p, q As Double

            dx = (xmax - xmin) / (a - 1);
            dy = (ymax - ymin) / (b - 1);

            for (int nx = 0; nx < a; nx++)
            {
                for (int ny = 0; ny < b; ny++)
                {
                    int k = 0;
                    Complex Z, C = new Complex();

                    //Find the current location
                    x0 = xmin + nx * dx;
                    y0 = ymin + ny * dy;
                    Z = new Complex(x0, y0);
                    C = new Complex(x0, y0);
                    r = 0;

                    while (k <= kmax && r < m)
                    {
                        Z = Complex.Pow(Z, 2) + C;
                        r = Complex.Abs(Z);
                        k += 1;
                    }

                    if (r > m)
                    {
                        images[ny, nx] = (int)k;
                    }
                    else
                    {
                        //The point didnt diverge 
                        images[ny, nx] = 0;
                    }
                    int Mask = 255 - images[ny, nx];// k;
                    byte gray = (byte)(Mask);
                    buffer.Add(gray);
                    buffer.Add(gray);
                    buffer.Add(gray);
                    buffer.Add(255);

                }
            }

          

            double dpiX = 96;
            double dpiY = 96;
            PixelFormat pixelFormat = PixelFormats.Pbgra32;
            int bytesPerPixel = (int)Math.Truncate((double)((pixelFormat.BitsPerPixel + 7) / 8));
            int stride = (int)(bytesPerPixel * images.GetLength(0));
            img.ImageSource = BitmapSource.Create(images.GetLength(0) - 1, images.GetLength(1) - 1, dpiX, dpiY, pixelFormat, null, buffer.ToArray(), stride);
            TranslateTransform translation = new TranslateTransform();
            translation.X = -300+xx;// 250;
            translation.Y = -250+yy;
            RotateTransform anotherRotateTransform = new RotateTransform();
            anotherRotateTransform.CenterX = 300;
            anotherRotateTransform.CenterY = 250;
            ScaleTransform s = new ScaleTransform();
            s.ScaleX = sca;
            s.ScaleY = sca;
            //int angle = int.Parse(kat_txt.Text.Replace(",", "."), CultureInfo.InvariantCulture);
            anotherRotateTransform.Angle = ann;
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(anotherRotateTransform);
            myTransformGroup.Children.Add(translation);
            myTransformGroup.Children.Add(s);

            img.Transform = myTransformGroup;
            canvas_draw.Background = img;
        }

        void rysuj_Julia(int xx, int yy, int ann, int sca, double jim, double jre)
        {

            double xmin, xmax, ymin, ymax = new double();
            xmin = -2;
            xmax = 2;
            ymin = -2;
            ymax = 2;
            ImageBrush img = new ImageBrush();
            List<byte> buffer = new List<byte>();
            int a, b, kmax, m = new int();
            a = 600;
            b = 600;
            kmax = 200;
            m = 4;
            int[,] images = new int[a, b];
            double x0, y0, dx, dy, x, y, r = new double();
            double p, q = new double();
            //Complex cc = new Complex(-0.74543, 0.11301);
            //cc.Real = -0.74543;
            p = jim;
            q = jre;
            // ' Dim p, q As Double

            dx = (xmax - xmin) / (a - 1);
            dy = (ymax - ymin) / (b - 1);

            for (int nx = 0; nx < a; nx++)
            {
                for (int ny = 0; ny < b; ny++)
                {
                    int k = 0;
                    Complex Z, C = new Complex();

                    //Find the current location
                    x0 = xmin + nx * dx;
                    y0 = ymin + ny * dy;
                    //Z = new Complex(x0, y0);
                    //C = new Complex(x0, y0);
                    r = 0;

                    while (k <= kmax && r < m)
                    {
                        x = x0;
                        y = y0;
                        x0 = x * x - y * y + p;
                        y0 = 2 * x * y + q;

                        r = x0 * x0 + y0 * y0;
                        k += 1;
                    }

                    images[ny, nx] = (int)k;
                    
                    int Mask = 255 - images[ny, nx];// k;
                    byte gray = (byte)(Mask);
                    buffer.Add(gray);
                    buffer.Add(gray);
                    buffer.Add(gray);
                    buffer.Add(255);

                }
            }



            double dpiX = 96;
            double dpiY = 96;
            PixelFormat pixelFormat = PixelFormats.Pbgra32;
            int bytesPerPixel = (int)Math.Truncate((double)((pixelFormat.BitsPerPixel + 7) / 8));
            int stride = (int)(bytesPerPixel * images.GetLength(0));
            img.ImageSource = BitmapSource.Create(images.GetLength(0) - 1, images.GetLength(1) - 1, dpiX, dpiY, pixelFormat, null, buffer.ToArray(), stride);
            TranslateTransform translation = new TranslateTransform();
            translation.X = -300 + xx;// 250;
            translation.Y = -300 + yy;
            RotateTransform anotherRotateTransform = new RotateTransform();
            anotherRotateTransform.CenterX = 300;
            anotherRotateTransform.CenterY = 300;
            
            anotherRotateTransform.Angle = ann;
            ScaleTransform s = new ScaleTransform();
            s.ScaleX = sca;
            s.ScaleY = sca;
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(anotherRotateTransform);
            myTransformGroup.Children.Add(translation);
            myTransformGroup.Children.Add(s);

            img.Transform = myTransformGroup;
            canvas_draw.Background = img;
        }

        private void Arrow_Click(object sender, MouseButtonEventArgs e)
        {
            canvas_draw.Children.Clear();

            x = (int)e.GetPosition(canvas_draw).X;
            y = (int)e.GetPosition(canvas_draw).Y;
            

            if (chck_d.IsChecked == true)
            {
                len_in = Int32.Parse(textBox1.Text);
                fraki.Add(Tuple.Create((double)len_in, x, y));
                drz_it = +1;
                list_it.Items.Add("drzewo");
                dodane.Add("drzewo");
            }else if(chck_t.IsChecked == true)
            {
                len_in = Int32.Parse(textBox1.Text);
                trojkaty.Add(Tuple.Create((double)len_in, x, y));
                list_it.Items.Add("trojkat");
                
                dodane.Add("trojkat");
                tr_it += 1;
            }
            else if(chck_j.IsChecked == true)
            {
                double j_im = Double.Parse(Jimag.Text);
                double j_re = Double.Parse(Jreal.Text);
                int j_sc = Int32.Parse(scale_txt.Text);
                int j_ang = Int32.Parse(kat_txt.Text);
                Julie.Clear();
                Mandy.Clear();
                Julie.Add(Tuple.Create(x, y, j_sc, j_ang, j_im, j_re));
            }
            else if (chck_m.IsChecked == true)
            {
                int j_sc = Int32.Parse(scale_txt.Text);
                int j_ang = Int32.Parse(kat_txt.Text);
                Julie.Clear();
                Mandy.Clear();
                Mandy.Add(Tuple.Create(x, y, j_sc, j_ang));
            }

            Draw_All();  
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            chck_t.IsChecked = false;
            chck_j.IsChecked = false;
            chck_m.IsChecked = false;

        }

        private void chck_t_Checked(object sender, RoutedEventArgs e)
        {
            chck_d.IsChecked = false;
            chck_j.IsChecked = false;
            chck_m.IsChecked = false;
        }

        private void chck_m_Checked(object sender, RoutedEventArgs e)
        {
            chck_t.IsChecked = false;
            chck_j.IsChecked = false;
            chck_d.IsChecked = false;
        }

        private void chck_j_Checked(object sender, RoutedEventArgs e)
        {
            chck_t.IsChecked = false;
            chck_d.IsChecked = false;
            chck_m.IsChecked = false;
        }

        private void list_it_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (list_it.SelectedItem.ToString() == "Drzewo")
            {
                textBox1.Text = fraki.
                len_in = Int32.Parse(textBox1.Text);
            }*/
        }
    }
}




