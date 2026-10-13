using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Juegos_de_cartas_21
{
    public partial class Form1 : Form
    {
        #region variables
        private string rutaBase = System.IO.Path.Combine(Application.StartupPath, "Resources") + @"\";
        private Dictionary<string, string> cartas;
        List<Carta> baraja = new List<Carta>();
        List<Carta> cartasJugador = new List<Carta>();
        List<Carta> cartasIA = new List<Carta>();
        int puntosJugador = 0;
        int puntosIA = 0;
        Random random = new Random();
        #endregion
        public Form1()
        {
            InitializeComponent();
            #region inicializacion de metodos
            CrearBaraja();
            MostrarDorso(pictureBox1);
            MostrarDorso(pictureBox2);
            MostrarDorso(pictureBox3);
            MostrarDorso(pictureBox4);
           
            hitMeButton.Enabled = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;    
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox13.Visible = false;
            pictureBox13.Parent = pictureBox12;
            #endregion

        }

        #region boton para iniciar el juego
        private void dealButton_Click(object sender, EventArgs e)
        {
            RepartirCartasIniciales();
            hitMeButton.Enabled = true;
        }
        #endregion

        #region metodo de crear la baraja
        private void CrearBaraja()
        {
            string[] palos = { "C", "D", "H", "S" }; // Trébol (C), Diamante (D), Corazón (H), Pica (S)
            string[] nombres = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] valores = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            foreach (var palo in palos)
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    baraja.Add(new Carta
                    {
                        Nombre = $"{nombres[i]} de {palo}",
                        Valor = valores[i],
                        Imagen = $"{nombres[i]}{palo}.png"
                    });
                }
            }
        }
        #endregion

        #region metodo de reinicio
        private void RepartirCartasIniciales()
        {
            cartasJugador.Clear();
            cartasIA.Clear();
            puntosJugador = 0;
            puntosIA = 0;

            cartasJugador.Add(SacarCarta(true)); 
            cartasJugador.Add(SacarCarta(true)); 
            cartasIA.Add(SacarCarta());         
            cartasIA.Add(SacarCarta());         

            ActualizarInterfaz();
        }


        #endregion

        #region metodo de sacar carta
        private Carta SacarCarta(bool esJugador = false)
        {
            // Asegúrate de que la baraja tenga cartas antes de intentar sacar una
            if (baraja.Count > 0)
            {
                int indice = random.Next(baraja.Count);
                Carta cartaSeleccionada = baraja[indice];
                baraja.RemoveAt(indice); // Elimina la carta de la baraja

                // Si la carta es un As y es del jugador, permite elegir el valor
                if (cartaSeleccionada.Valor == 11 && esJugador)
                {
                    cartaSeleccionada.Valor = ElegirValorAs(); // Método para elegir el valor del As
                }

                return cartaSeleccionada;
            }
            else
            {
                MessageBox.Show("No hay más cartas en la baraja.");
                return null; // Si no hay más cartas en la baraja, devuelve null
            }
        }




        #endregion

        #region actualizar la interfaz
        private void ActualizarInterfaz()
        {
            pictureBox1.ImageLocation = $"{rutaBase}{cartasJugador[0].Imagen}";
            pictureBox2.ImageLocation = $"{rutaBase}{cartasJugador[1].Imagen}";
            pictureBox3.ImageLocation = $"{rutaBase}{cartasIA[0].Imagen}";
            pictureBox4.ImageLocation = $"{rutaBase}b1fv.png";

            puntosJugador = CalcularPuntos(cartasJugador);
            puntosIA = CalcularPuntos(cartasIA);
            labelJugador.Text = $"Puntos: {puntosJugador}";
        }
        #endregion

        #region metodo para mostrar el revez de la carta
        private void MostrarDorso(PictureBox pictureBox)
        {
            pictureBox.ImageLocation = $"{rutaBase}b1fv.png";
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        #endregion

        #region metodo de calcular puntos 
        private int CalcularPuntos(List<Carta> cartas)
        {
            int suma = 0;
            int ases = 0;

            foreach (var carta in cartas)
            {
                suma += carta.Valor;
                if (carta.Valor == 11) ases++;
            }

            while (suma > 21 && ases > 0)
            {
                suma -= 10;
                ases--;
            }

            return suma;
        }
        #endregion

        #region metodo de reinicar el juego 
        private void ReiniciarJuego()
        {
            CrearBaraja();
            MostrarDorso(pictureBox1);
            MostrarDorso(pictureBox2);
            MostrarDorso(pictureBox3);
            MostrarDorso(pictureBox4);
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            pictureBox10.Image = null;

            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox13.Visible = false;


            puntosJugador = 0;
            puntosIA = 0;

            labelJugador.Text = "Puntos: 0";
            hitMeButton.Enabled = false;
        }
        #endregion


        #region bton de plantarse
        private void button1_Click(object sender, EventArgs e)
        {
            while (puntosIA < 17 || (puntosIA < puntosJugador && puntosJugador <= 21))
            {
                Carta cartaIA = SacarCarta();
                cartasIA.Add(cartaIA);
                puntosIA = CalcularPuntos(cartasIA);
                ActualizarImagenIA();
            }

            MostrarResultado();
            ReiniciarJuego();
            hitMeButton.Enabled = false;
        }
        #endregion

        private void MostrarResultado()
        {
            string cartasIAReveladas = "Cartas de la IA: \n";
            foreach (var carta in cartasIA)
            {
                cartasIAReveladas += $"{carta.Nombre} (Valor: {carta.Valor})\n";
            }

            // Mostrar los puntos finales de la IA y del jugador
            string resultado = $"La IA tiene {puntosIA} puntos.\n";

            // Determinar el ganador
            if (puntosIA > 21 || puntosJugador > puntosIA)
            {
                pictureBox13.Image = Image.FromFile(rutaBase + "1V8t.gif"); // Imagen de victoria
                pictureBox13.Visible = true;

                MessageBox.Show($"¡Ganaste!\n{resultado}");
            }
            else if (puntosIA == puntosJugador)
            {
                MessageBox.Show($"¡Empate!\n{resultado}");
            }
            else
            {
                pictureBox13.Visible = true;
                pictureBox13.Image = Image.FromFile(rutaBase + "7V5.gif"); // Imagen de derrota

                MessageBox.Show($"La IA gana.\n{resultado}");
            }

            ReiniciarJuego();
        }


        #region bton de jalar
        private void hitMeButton_Click(object sender, EventArgs e)
        {
            if (puntosJugador < 21)
            {
                Carta cartaJugador = SacarCarta(true);
                cartasJugador.Add(cartaJugador);
                puntosJugador = CalcularPuntos(cartasJugador);
                ActualizarImagenJugador(cartaJugador);
                labelJugador.Text = $"Puntos: {puntosJugador}";

                if (puntosJugador > 21) // Jugador pierde automáticamente
                {
                    pictureBox13.Visible = true;
                    pictureBox13.Image = Image.FromFile(rutaBase + "7V5.gif"); // Imagen de derrota
                    MessageBox.Show("¡Te pasaste de 21! La IA gana.");
                    ReiniciarJuego();
                }
            }
        }
        #endregion
        private void ActualizarImagenJugador(Carta cartaJugador)
        {
            int playerBoxIndex = cartasJugador.Count - 1;
            PictureBox pBox = ObtenerSiguientePictureBoxJugador(playerBoxIndex);

            if (pBox != null)
            {
                pBox.ImageLocation = $"{rutaBase}{cartaJugador.Imagen}";
                pBox.Visible = true; // Hacer visible el PictureBox correspondiente
            }
        }


        private PictureBox ObtenerSiguientePictureBoxJugador(int index)
        {
            List<PictureBox> pictureBoxesJugador = new List<PictureBox> { pictureBox1, pictureBox2, pictureBox5, pictureBox6, pictureBox7 };
            return pictureBoxesJugador.ElementAtOrDefault(index);
        }

        private void TurnoIA()
        {
            while (puntosIA < 17 || (puntosIA < puntosJugador && puntosJugador <= 21))
            {
                Carta cartaIA = SacarCarta();
                cartasIA.Add(cartaIA);
                puntosIA = CalcularPuntos(cartasIA);
                ActualizarImagenIA();
            }

            MostrarResultado();
        }

        private void ActualizarImagenIA()
        {
            if (cartasIA.Count > 0)
            {
                int iaBoxIndex = cartasIA.Count - 1;
                PictureBox pBox = ObtenerSiguientePictureBoxIA(iaBoxIndex);

                if (pBox != null)
                {
                    pBox.ImageLocation = $"{rutaBase}{cartasIA.Last().Imagen}";
                    pBox.Visible = true; // Hacer visible el PictureBox correspondiente
                }
            }
        }


        private PictureBox ObtenerSiguientePictureBoxIA(int index)
        {
            List<PictureBox> pictureBoxesIA = new List<PictureBox> { pictureBox3, pictureBox4, pictureBox8, pictureBox9, pictureBox10 };
            return pictureBoxesIA.ElementAtOrDefault(index);
        }

        private int ElegirValorAs()
        {
            var resultado = MessageBox.Show(
                "¡Has recibido un As! ¿Quieres que valga 11 puntos?",
                "Elección del As",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            return resultado == DialogResult.Yes ? 11 : 1;
        }

    
}
    #region clase de cartas
    public class Carta
    {
        public string Nombre { get; set; }
        public int Valor { get; set; }
        public string Imagen { get; set; }
    }
    #endregion
}

