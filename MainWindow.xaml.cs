/*
 * Brad Wells
 * CIS 229.2258
 * 3/22/19
 * Assignment C
 * This is a card matching game made with WPF
 *
 * The game works but I can not get the cards to flip back around if they do not match. I have left in comments where I
 * have tried timers or thread.sleep. I have created another way to visually track matches, however it is not exactally what
 * the assignment asks for. 
 */

//how to put a timer or sleep if the cards are wrong?
using AsgC.Recources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AsgC
{
           


    public partial class MainWindow : Window
    {
        ArrayList selectedCard = new ArrayList();
       // List<Card> = new List<Card();
        ArrayList disabledCards = new ArrayList();
        List<Card> sC = new List<Card>();
        List<Card> dC = new List<Card>();
        Boolean found = false;
        List<Card> aList = new List<Card>();
        List<Card> bList = new List<Card>();
        String aa;
        String bb;
        int total;
        int pickedCards = 0;
        int clicks;
        int round = 1;


        //public class Timers
        //{
        //    private DispatcherTimer t;
        //    private TimeSpan ts;

        //    public TimeSpan Time
        //    {
        //        get
        //        {
        //            return ts;
        //        }
        //        set
        //        {
        //            ts = value;
        //        }
        //    }

        //    public Timers(TimeSpan ts)
        //    {
        //        t = new DispatcherTimer();
        //        t.Interval = ts;
        //        t.Tick += PlayedTimer_Tick;

        //    }

        //    public Timers()
        //    {
        //    }

        //    public void Start()
        //    {
        //        t.Start();
        //    }
        //    public void Stop()
        //    {
        //        t.Stop();
        //    }

        //    private void PlayedTimer_Tick(object sender, EventArgs e)
        //    {
        //        Time = ts.Add(new TimeSpan(0, 0, 1));
        //    }
        //}







        //Define rows and cols
        int[] rows =  { 2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5,6,6,6,6,7,7,7,7,8,8,8,8};

        int[] cols =  { 2,3,4,5,2,3,4,5,2,3,4,5,2,3,4,5,2,3,4,5,2,3,4,5,2,3,4,5};

        // string of names that match the file names of each img
        String[] names = { "Frank1", "Frank2", "Frank3", "Frank4", "Frank5", "Frank6", "Frank7", "Frank8", "Frank1", "Frank2", "Frank3", "Frank4", "Frank5", "Frank6", "Frank7", "Frank8" };


        // this is a method to shuffle the list so that each game will have a different grid
        void Shuffle(string[] name)
        {
            for (int t = 0; t < name.Length; t++)
            {
                string tmp = name[t];
                Random rand = new Random(names.Length);
                int r = rand.Next(names.Length);
                name[t] = name[r];
                name[r] = tmp;
            }
        } // end Shuffle


        //*********************************************************************************************************************************************************************************
        public MainWindow()
        {
            InitializeComponent();


            Shuffle(names);
            
            //loop through the names and make each name an instance of class Card
            for(int i=0; i<16; i++)
            {
                Card c = new Card(names[i], "back2");
                c.Click += new RoutedEventHandler(cardSelected);
                //c.Click += new RoutedEventHandler(checkMatch);

                //set locations
                Grid.SetColumn(c, cols[i]);
                Grid.SetRow(c, rows[i]);
                c.DisplayBack();
                CardGrid.Children.Add(c);
            }




        } // end MainWindow Constructor


        //*********************************************************************************************************************************************************************************
        //This method is called from the click event handler each time a card is clicked. It will keep track of how many are clicked ( 1 or 2 ), display the back side of the card, and add
        //selected cards to a list to keep track of them. 
        public void cardSelected(object sender, RoutedEventArgs e)
        {
            pickedCards += 1;
            clicks += 1;
            Clicks.Text = clicks.ToString();

            Card c = (Card)sender;
            c.DisplayFront();


            // Keep track of picked cards
            // if one card is picked, add it to list a
            if (pickedCards == 1)
            {
                aList.Add(c);
                aa=(c.ToString());
            }
            // If two cards are picked, add the second to list b and compare them
            if (pickedCards == 2)
            {
                bList.Add(c);
                bb=(c.ToString());

                // calling compare method
                compare(aa, bb);
            }

            //if(pickedCards == 2)
            //{
            //    //String aa;
            //    //String bb;
            //    //foreach(Card x in a)
            //    //{
            //    //    aa = x.Name;
            //    //}
            //    //foreach(Card g in b)
            //    //{
            //    //    bb = g.Name;
            //    //}
            //    compare(aa, bb);
            //}


            // when all 8 guesses(16 clicks) have been tried, end game
            if (clicks == 16)
            {
               // If they matched all cards
                if (total == 8)
                {
                    
                  var result =   MessageBox.Show("Nice job, Jabroni! \nPlay again? ", "End of Round "+round.ToString(),MessageBoxButton.YesNo);
                  if (result == MessageBoxResult.No)
                  {
                      this.Close();
                  }
                  else
                  {
                      PlayAgain();
                  }
                }
                // If not all cards are matched
                else
                {
                   
                    var result = MessageBox.Show("Tough luck, Bozo! \nPlay again?", "End of Round " + round.ToString(), MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        PlayAgain();
                    }
                }
            }



        } // end cardSelected

        //*********************************************************************************************************************************************************************************

        //When two cards are selected, this method is called and checks for a match. 
        private void compare(String a, String b)
        {
            //  MessageBox.Show("a: " + a + "B" + b);

            if (a==b)
            {
               // MessageBox.Show("match");
                disableWinCards();
                total += 1;

                //Score.Text = (total).ToString();
                pickedCards = 0;                      //reset picked cards 
            }
            else
            {
                // MessageBox.Show("no match");

               // Thread.Sleep(1000);
                disableLossCards();

               // Thread.Sleep(1000);
                //foreach (Card n in aList )
                //{
                //    n.DisplayBack();
                //}

                //foreach (Card m in bList )
                //{
                //    m.DisplayBack();
                //}

                aList.Clear();                       //clear list to try again, 
                bList.Clear();
                pickedCards = 0;                     //reset picked cards
            }
        } // end compare

        //*********************************************************************************************************************************************************************************

        //This method  is called when the cards match. It changes the opacity and disables them from being clicked again
        private void disableWinCards()
        {
            foreach(Card j in aList)
            {
                j.Opacity = 0.3;
                j.IsEnabled = false;
            }

            foreach(Card k in bList)
            {
                k.Opacity = 0.3;
                k.IsEnabled = false;
            }
        } // end disabledWinCards






        //This method is called when the cards do not match. It disables them from being clicked again, but dot not change the opacity
        private void disableLossCards()
        {
            //Timers t = new Timers();
            //t.Start();
            //Thread.Sleep(1000);
            foreach (Card j in aList )
            {

                j.IsEnabled = false;
                //j.DisplayFront();
                //Task.Delay(TimeSpan.FromSeconds(1));
                
                //Thread.Sleep(1000);
               // j.DisplayBack();
            }


            foreach (Card k in bList)
            {
                //k.DisplayFront();
                //Task.Delay(TimeSpan.FromSeconds(1));

                 k.IsEnabled = false;

                 //Thread.Sleep(1000);
                // k.DisplayBack();
                
            }
            //Thread.Sleep(1000);
            
        }

        //*********************************************************************************************************************************************************************************

        //Not currently using
        private bool checkMatch(String card, Card c)
        {
            //if (selectedCard.Count < 2)
            //{
            //    return;
            //}

            if (selectedCard.Contains(card))
            {
                MessageBox.Show("match" + card);
                sC.Add(c);

                String y = "sC list: ";
                String z = "dC list: ";

                foreach (Card x in sC)
                {
                    y+=x.ToString()+"";
                    dC.Add(x);
                   
                    x.IsEnabled = false;
                    x.Opacity = 0.3;
                }
                MessageBox.Show(y);

                foreach (Card x in dC)
                {
                    z += x.ToString();
                   // c.IsEnabled = false;
                    c.Opacity = 0.3;
                }

                MessageBox.Show(z);
                selectedCard.Clear();
                sC.Clear();
                dC.Clear();
                
                return true;
            }
            else
            {

                selectedCard.Clear();
                sC.Clear();
                dC.Clear();
                return false;
            }
        } // end CheckMatch


        //*********************************************************************************************************************************************************************************

        //This method will reset the counters and start a new grid if the user chooses to play again
        public void PlayAgain()
        {
            total = 0;
            clicks = 0;
            Clicks.Text = "";
            round += 1;
            Round.Text = round.ToString();

            Shuffle(names);
            
            CardGrid.Children.Clear();

            


            //loop through the names and make each name an instance of class Card
            for (int i = 0; i < 16; i++)
            {
                Card c = new Card(names[i], "back2");
                c.Click += new RoutedEventHandler(cardSelected);
                //c.Click += new RoutedEventHandler(checkMatch);

                //set locations
                Grid.SetColumn(c, cols[i]);
                Grid.SetRow(c, rows[i]);
                c.DisplayBack();
                CardGrid.Children.Add(c);
            }
        } // end PlayAgain


    } //End class MainWindow

} // end namespace AsgC
