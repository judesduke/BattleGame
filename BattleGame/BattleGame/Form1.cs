using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;

namespace BattleGame
{

    public partial class frmBattleGame : Form
    {

        //private SoundPlayer themeMusic;
        private SoundPlayer rupAtkSound;
        private SoundPlayer flamethowerAtkSound;
        private SoundPlayer cursorHover;
        private SoundPlayer cursorError;
        private SoundPlayer hyperBeamAtkSound;
        private SoundPlayer playerHealSound;
        private SoundPlayer compHealSound;
        private SoundPlayer stabAtkSound;
        private SoundPlayer shurAtkSound;
        private SoundPlayer crunchAtkSound;

        //Variables
        string turn;
        int randomTurnGen;
        int randomCompMove;
        bool isUserFirst;
        int uTotalDmgDealt;
        int uTotalDmgTaken;
        int uTotalHealthGained;
        int cTotalDmgDealt;
        int cTotalDmgTaken;
        int playerStabAtk;
        int playerRupAtk;
        int playerShurAtk;
        int playerHealAmount;
        int compCrunchAtk;
        int compFlamethrowerAtk;
        int compHyperBeamAtk;
        int compRegenAmount;
        int compHealth;
        int currentCompHP;
        //int currentCompHPDmg;
        int currentUserHP;
        int userHealth;
        bool turnSystem;
        int monsterTurn;
        bool didStabCrit;
        bool didRupCrit;
        bool didShurCrit;
        bool didHealCrit;
        bool didCrunchCrit;
        bool didFlamethrowerCrit;
        bool didHyperBeamCrit;
        bool didRegenCrit;
        int score;
        int pointDmgTaken;
        int pointHeals;
        int pointDmgDealt;

        public frmBattleGame()
        {
            InitializeComponent();


        }

        private void frmBattleGame_Load(object sender, EventArgs e)
        {
            //gets theme music then plays it
            //themeMusic = new SoundPlayer("FinalFantasy4Boss.wav");
            //themeMusic.PlayLooping();

            //Runs the program that plays the background music
            Process.Start("PlayingSounds.exe");

            //Button sound effects
            rupAtkSound = new SoundPlayer("RuptureAttackSound1.wav");
            flamethowerAtkSound = new SoundPlayer("FlamethowerSound1.wav");
            cursorHover = new SoundPlayer("CursorHover1.wav");
            cursorError = new SoundPlayer("CursorError1.wav");
            hyperBeamAtkSound = new SoundPlayer("HyperBeam1.wav");
            playerHealSound = new SoundPlayer("Heal2.wav");
            compHealSound = new SoundPlayer("Regen1.wav");
            stabAtkSound = new SoundPlayer("Stab1.wav");
            shurAtkSound = new SoundPlayer("Shuriken2.wav");
            crunchAtkSound = new SoundPlayer("Crunch.wav");

            //Base Colors for buttons
            btnStab.BackColor = Color.Navy;
            btnRupture.BackColor = Color.Navy;
            btnShuriken.BackColor = Color.Navy;
            btnHeal.BackColor = Color.Navy;
            btnCrunch.BackColor = Color.Navy;
            btnFlamethrower.BackColor = Color.Navy;
            btnHyperBeam.BackColor = Color.Navy;
            btnRegen.BackColor = Color.Navy;
            btnReset.BackColor = Color.Black;
            btnStartGame.BackColor = Color.White;
            btnHelpPlay.BackColor = Color.Black;
        }

        //Buttons
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            //Restarts the timer that keeps track of the score            
            scoreUpdater.Enabled = true;

            //Displays play screen
            pnlPlay.Visible = true;

            //Generates User's HP then displays it
            Random userHPGen = new Random();
            userHealth = userHPGen.Next(75, 151);
            currentUserHP = userHealth;
            lblUserHealth.Text = "HP " + userHealth + " / " + userHealth;

            //Generates Computer's HP then displays it
            Random compHPGen = new Random();
            compHealth = compHPGen.Next(50, 101);
            currentCompHP = compHealth;
            lblCompHealth.Text = "HP " + compHealth + " / " + compHealth;

            //Finds who is first , then puts turn system in place
            isUserFirst = FirstTurn();
            if (isUserFirst == true)
            {
                btnStab.Enabled = true;
                btnRupture.Enabled = true;
                btnShuriken.Enabled = true;
                btnHeal.Enabled = true;
                turnSystem = true;               
            }
            else
            {
                btnStab.Enabled = false;
                btnRupture.Enabled = false;
                btnShuriken.Enabled = false;
                btnHeal.Enabled = false;
                turnSystem = true;
                monsterTurn = MonsterAI();
                lblWhosTurn.Text = " Player's Turn";             
            }

            lblScore.Text = "Score: " + Convert.ToString(score);          
        }
        

        private void btnStab_Click(object sender, EventArgs e)
        {
            

            //Random Attack Dmg
            Random stabAtkDmg = new Random();

            //finds if there was a crit or not
            int critDmg;

            /*Checks if the move has hit using the CheckSucces subprogram , if the user hits it calculates the crit
             * 
             * 
             */
            bool hitOrMiss = CheckSuccess(70);
            if (hitOrMiss == true)
            {
                playerStabAtk = stabAtkDmg.Next(15, 31);
                
                didStabCrit = CriticalHit(playerStabAtk);

                stabAtkSound.Play();
                if (didStabCrit == true)
                {
                    critDmg = (playerStabAtk * 2);
                    currentCompHP = currentCompHP - critDmg;
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    if (critDmg == 0)
                    {
                        MessageBox.Show("You use Poison Stab! Your Poison Stab ability missed the target! ");
                    }
                    else
                    {
                        MessageBox.Show("You use Poison Stab! Poison Stab has Crit for " + critDmg + " Damage!");
                    }
                    
                }
                else
                {
                    critDmg = playerStabAtk;
                    currentCompHP = currentCompHP - critDmg;
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    MessageBox.Show("You use Poison Stab! You stab the monster for " + Convert.ToString(playerStabAtk) + " Damage!");
                }
            }
            else
            {
                playerStabAtk = 0;
                //uTotalDmgDealt = uTotalDmgDealt + playerStabAtk;
                MessageBox.Show("You use Poison Stab! Your Poison Stab ability missed the target! ");
            }

            

            lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
            //finds total dmg dealt

            cTotalDmgTaken = uTotalDmgDealt;

            ////////score
            pointDmgDealt = (uTotalDmgDealt * 2);
            //////score = pointDmgDealt ;
            

            //uTotalDmgDealt = uTotalDmgDealt + critDmg;
            //cTotalDmgTaken = uTotalDmgDealt;
            lblUserDmgGiv.Text = ("Player Damage Dealt: " + Convert.ToString(uTotalDmgDealt));
            lblCompDmgTaken.Text = ("Monster Damage Taken: " + Convert.ToString(cTotalDmgTaken));
            //Change the hp of the comp based on this atk
            //currentCompHP = currentCompHP - playerStabAtk;
            lblCompHealth.Text = "HP " + currentCompHP + " / " + compHealth;


            //turn system
            if (turnSystem == true)
            {
                //isUserFirst = true;
                turnSystem = true;
                btnStab.Enabled = false;
                btnRupture.Enabled = false;
                btnShuriken.Enabled = false;
                btnHeal.Enabled = false;
                //MessageBox.Show("MONSTER's TURN");
                lblWhosTurn.Text = " Monster's Turn";
                monsterTurn = MonsterAI();
                //MessageBox.Show("PLAYER's TURN");
                lblWhosTurn.Text = " Player's Turn";
            }

            //else
            //{
            //    //isUserFirst = false;
            //    turnSystem = false;
            //    btnStab.Enabled = false;
            //    btnRupture.Enabled = false;
            //    btnShuriken.Enabled = false;
            //    btnHeal.Enabled = false;
            //    MessageBox.Show("MONSTER's TURN");
            //    monsterTurn = MonsterAI();

            //}
            //MessageBox.Show(Convert.ToString(turnSystem));



        }

        private void btnRupture_Click(object sender, EventArgs e)
        {
            
            
            

            //Finds Attack Dmg
            Random rupAtkDmg = new Random();

            //finds if there was a crit or not
            int critDmg;
            //didRupCrit = CriticalHit(playerRupAtk);
            //if (didRupCrit == true)
            //{
            //    critDmg = (playerRupAtk * 2);
            //    currentCompHP = currentCompHP - critDmg;

            //}
            //else
            //{
            //    critDmg = playerRupAtk;
            //    currentCompHP = currentCompHP - playerRupAtk;
            //}


            //was the user hit or missed, then gives misses a value of 0
            bool hitOrMiss = CheckSuccess(50);
            if (hitOrMiss == true)
            {
                rupAtkSound.Play();
                playerRupAtk = rupAtkDmg.Next(25, 41);               
                didRupCrit = CriticalHit(playerRupAtk);
                
                
                if (didRupCrit == true)
                {
                    critDmg = (playerRupAtk * 2);
                    currentCompHP = currentCompHP - critDmg;
                    
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    if (critDmg == 0)
                    {
                        MessageBox.Show("Your Rupture ability missed the target! ");
                    }
                    else
                    {
                        MessageBox.Show("Rupture has Crit for " + critDmg + " Damage!");
                    }
                }
                else
                {
                    critDmg = playerRupAtk;
                    currentCompHP = currentCompHP - critDmg;
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    MessageBox.Show("You slice the monster for " + Convert.ToString(playerRupAtk) + " Damage!");
                }
            }
            else
            {
                playerRupAtk = 0;
                uTotalDmgDealt = uTotalDmgDealt + playerRupAtk;
                MessageBox.Show("Your Rupture ability missed the target! ");
            }

            lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
            //finds total dmg dealt
            cTotalDmgTaken = uTotalDmgDealt;

            //score
            pointDmgDealt = (uTotalDmgDealt * 2);
            //pointDmgDealt + score = score;
            //lblScore.Text = "Score: " + Convert.ToString(score);

            lblUserDmgGiv.Text = ("Player Damage Dealt: " + Convert.ToString(uTotalDmgDealt));
            lblCompDmgTaken.Text = ("Monster Damage Taken: " + Convert.ToString(cTotalDmgTaken));
            //Change the hp of the comp based on this atk
            //currentCompHP = currentCompHP - playerRupAtk;
            lblCompHealth.Text = "HP " + currentCompHP + " / " + compHealth;

            //turn system
            if (turnSystem == true)
            {
                //isUserFirst = true;
                turnSystem = true;
                btnStab.Enabled = false;
                btnRupture.Enabled = false;
                btnShuriken.Enabled = false;
                btnHeal.Enabled = false;
                //MessageBox.Show("MONSTER's TURN");
                lblWhosTurn.Text = " Monster's Turn";
                monsterTurn = MonsterAI();
                //MessageBox.Show("PLAYER's TURN");
                lblWhosTurn.Text = " Player's Turn";
            }

            //if (turnSystem == true)
            //{
            //    isUserFirst = true;
            //    btnStab.Enabled = false;
            //    btnRupture.Enabled = false;
            //    btnShuriken.Enabled = false;
            //    btnHeal.Enabled = false;
            //    turnSystem = true;
            //    MessageBox.Show("MONSTER's TURN");
            //    monsterTurn = MonsterAI();
            //}
            //else
            //{
            //    isUserFirst = false;
            //    MessageBox.Show("PLAYER's TURN");

            //}
        }

        private void btnShuriken_Click(object sender, EventArgs e)
        {
            //Finds Attack Dmg
            Random shurAtkDmg = new Random();

            //finds if there was a crit or not
            int critDmg;
            //didShurCrit = CriticalHit(playerShurAtk);
            //if (didShurCrit == true)
            //{
            //    critDmg = (playerShurAtk * 2);
            //    currentCompHP = currentCompHP - critDmg;

            //}
            //else
            //{
            //    critDmg = playerShurAtk;
            //    currentCompHP = currentCompHP - playerShurAtk;
            //}



            //was the user hit or missed, then gives misses a value of 0
            bool hitOrMiss = CheckSuccess(30);
            if (hitOrMiss == true)
            {

                playerShurAtk = shurAtkDmg.Next(25, 61);
                
                didShurCrit = CriticalHit(playerShurAtk);
                shurAtkSound.Play();
                ;
                if (didShurCrit == true)
                {
                    critDmg = (playerShurAtk * 2);
                    currentCompHP = currentCompHP - critDmg;
                    
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    if (critDmg == 0)
                    {
                        MessageBox.Show("Your Shuriken ability missed the target! ");
                    }
                    else
                    {
                        MessageBox.Show("Shuriken has Crit for " + critDmg + " Damage!");
                    }
                }
                else
                {
                    critDmg = playerShurAtk;
                    currentCompHP = currentCompHP - critDmg;
                    uTotalDmgDealt = uTotalDmgDealt + critDmg;
                    MessageBox.Show("Your star hits the monster for " + Convert.ToString(playerShurAtk) + " Damage!");
                }
                

            }
            else
            {
                playerShurAtk = 0;
                MessageBox.Show("Your Shuriken ability missed the target! ");
            }

            lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
            //finds total dmg dealt
            cTotalDmgTaken = uTotalDmgDealt;

            //score
            pointDmgDealt = (uTotalDmgDealt * 2);
            //pointDmgDealt + score = score;
            //lblScore.Text = "Score: " + score;

            lblUserDmgGiv.Text = ("Player Damage Dealt: " + Convert.ToString(uTotalDmgDealt));
            lblCompDmgTaken.Text = ("Monster Damage Taken: " + Convert.ToString(cTotalDmgTaken));
            //Change the hp of the comp based on this atk
            //currentCompHP = currentCompHP - playerShurAtk;
            lblCompHealth.Text = "HP " + currentCompHP + " / " + compHealth;

            //turn system
            if (turnSystem == true)
            {
                //isUserFirst = true;
                turnSystem = true;
                btnStab.Enabled = false;
                btnRupture.Enabled = false;
                btnShuriken.Enabled = false;
                btnHeal.Enabled = false;
                //MessageBox.Show("MONSTER's TURN");
                lblWhosTurn.Text = " Monster's Turn";
                monsterTurn = MonsterAI();
                //MessageBox.Show("PLAYER's TURN");
                lblWhosTurn.Text = " Player's Turn";
            }


            //if (turnSystem == true)
            //{
            //    isUserFirst = true;
            //    btnStab.Enabled = false;
            //    btnRupture.Enabled = false;
            //    btnShuriken.Enabled = false;
            //    btnHeal.Enabled = false;
            //    turnSystem = true;
            //    MessageBox.Show("MONSTER's TURN");
            //    monsterTurn = MonsterAI();
            //}
            //else
            //{
            //    isUserFirst = false;
            //    MessageBox.Show("PLAYER's TURN");

            //}
        }

        private void btnHeal_Click(object sender, EventArgs e)
        {


            //Finds Attack Dmg
            Random healAmount = new Random();

            //finds if there was a crit or not
            int critDmg;
            //didHealCrit = CriticalHit(playerHealAmount);
            //if (didHealCrit == true)
            //{
            //    critDmg = (playerHealAmount * 2);
            //    currentCompHP = currentCompHP - critDmg;

            //}
            //else
            //{
            //    critDmg = playerHealAmount;
            //    currentCompHP = currentCompHP - playerHealAmount;
            //}

            
            //was the user hit or missed

            bool hitOrMiss = CheckSuccess(70);
            if (hitOrMiss == true)
            {
                playerHealAmount = healAmount.Next(20, 81);
                didHealCrit = CriticalHit(playerHealAmount);
                playerHealSound.Play();

                if (didHealCrit == true)

                {
                    critDmg = (playerHealAmount * 2);
                    uTotalHealthGained = uTotalHealthGained + critDmg;
                    MessageBox.Show("your Heal has Crit for " + critDmg + " Hit Points!");
                    //if (critDmg == 0)
                    //{

                    //    MessageBox.Show("Your Rejuvination ability missed! ");
                    //}
                    //else
                    //{
                        
                    //    MessageBox.Show("This move has Crit for " + critDmg + " Damage!");
                    //}
                }
                else
                {
                    critDmg = playerHealAmount;
                    uTotalHealthGained = uTotalHealthGained + critDmg;
                    MessageBox.Show("You heal your self for " + critDmg + " Hit Points!");
                }
                

            }
            else
            {
                critDmg = playerHealAmount;
                critDmg = 0;
                MessageBox.Show("Your Rejuvination ability missed! ");
            }

            //Healing Cap
            if (currentUserHP + critDmg > userHealth)
            {
                currentUserHP = userHealth;

            }
            else
            {
                currentUserHP = currentUserHP + critDmg;
            }

            pointHeals = uTotalHealthGained;
            //uTotalHealthGained = score;
            //lblScore.Text = "Score:Heal " + Convert.ToString(uTotalHealthGained);

            lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;

            //turn system
            if (turnSystem == true)
            {
                //isUserFirst = true;
                turnSystem = true;
                btnStab.Enabled = false;
                btnRupture.Enabled = false;
                btnShuriken.Enabled = false;
                btnHeal.Enabled = false;
                //MessageBox.Show("MONSTER's TURN");
                lblWhosTurn.Text = " Monster's Turn";
                monsterTurn = MonsterAI();
                //MessageBox.Show("PLAYER's TURN");
                lblWhosTurn.Text = " Player's Turn";
            }



            //if (turnSystem == true)
            //{
            //    isUserFirst = true;
            //    btnStab.Enabled = false;
            //    btnRupture.Enabled = false;
            //    btnShuriken.Enabled = false;
            //    btnHeal.Enabled = false;
            //    turnSystem = true;
            //    MessageBox.Show("MONSTER's TURN");
            //    monsterTurn = MonsterAI();

            //}
            //else
            //{
            //    isUserFirst = false;
            //    MessageBox.Show("PLAYER's TURN");
            //}
        }

        //Subprograms

        //Monster AI 
        private int MonsterAI()
        {

            if (currentCompHP < (compHealth * 0.25))
            {
                 bool hitOrMiss = CheckSuccess(40);
                 if (hitOrMiss == true)
                 {
                     compHealSound.Play();
                     int critDmg;
                     Random regenAmount = new Random();
                     compRegenAmount = regenAmount.Next(30, 61);
                     didRegenCrit = CriticalHit(compRegenAmount);
                     if (didRegenCrit == true)
                     {
                         //crit
                         critDmg = (compRegenAmount * 2);                         
                         if (critDmg == 0)
                         {
                             MessageBox.Show("The Monsters Heal ability missed! ");
                         }
                         else
                         {
                             MessageBox.Show("The Monster's Heal has Crit for " + critDmg + " Hit Points!");
                         }

                     }
                     else
                     {
                         critDmg = compRegenAmount; 
                         MessageBox.Show("The Monster Heals for " + Convert.ToString(compRegenAmount) + " Hit Points!");
                     }

                     //healing cap
                     if (currentCompHP + critDmg > compHealth)
                     {
                         currentCompHP = compHealth;

                     }
                     else
                     {
                         currentCompHP = currentCompHP + critDmg;
                     }

                     
                     turnSystem = true;
                     btnStab.Enabled = true;
                     btnRupture.Enabled = true;
                     btnShuriken.Enabled = true;
                     btnHeal.Enabled = true;


                     lblCompHealth.Text = "HP " + currentCompHP + " / " + compHealth;

                     return compRegenAmount;
                 }
                 else
                 {
                     compRegenAmount = 0;
                     MessageBox.Show("The Monsters Heal ability missed! ");
                     turnSystem = true;
                     btnStab.Enabled = true;
                     btnRupture.Enabled = true;
                     btnShuriken.Enabled = true;
                     btnHeal.Enabled = true;
                     return compRegenAmount;
                 }
            }
            else
            {
                Random generator = new Random();
                randomCompMove = generator.Next(1, 4);

                if (randomCompMove == 1)
                {
                    crunchAtkSound.Play();
                    bool hitOrMiss = CheckSuccess(75);
                    if (hitOrMiss == true)
                    {
                        int critDmg;

                        Random crunchAtk = new Random();
                        compCrunchAtk = crunchAtk.Next(20, 36);
                        didCrunchCrit = CriticalHit(compCrunchAtk);

                        if (didCrunchCrit == true)
                        {
                            critDmg = (compCrunchAtk * 2);
                            currentUserHP = currentUserHP - critDmg;

                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            if (critDmg == 0)
                            {
                                MessageBox.Show("The Monsters Crunch ability missed! ");
                            }
                            else
                            {
                                MessageBox.Show("Crunch has Crit for " + critDmg + " Damage!");
                            }
                        }
                        else
                        {
                            critDmg = compCrunchAtk;
                            currentUserHP = currentUserHP - critDmg;
                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            MessageBox.Show("The Monster attacks you with Crunch for " + Convert.ToString(compCrunchAtk) + " Damage!");
                        }

                        
                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;

                        //Change the hp of the comp based on this atk
                        //currentUserHP = currentUserHP - compCrunchAtk;
                        lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
                        uTotalDmgTaken = cTotalDmgDealt;
                        pointDmgTaken = (-1) * (uTotalDmgTaken);
                        //pointDmgTaken + score = score;
                        //lblScore.Text = "Score: " + score;

                        lblCompDmgGiv.Text = ("Monster Damage Dealt: " + Convert.ToString(cTotalDmgDealt));
                        lblUserDmgTaken.Text = ("Player Damage Taken: " + Convert.ToString(uTotalDmgTaken));
                        
                        
                        return compCrunchAtk;
                    }
                    else
                    {
                        compCrunchAtk = 0;
                        MessageBox.Show("The Monsters Crunch ability missed! ");
                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;
                        return compCrunchAtk;
                    }
                }
                else if (randomCompMove == 2)
                {
                    flamethowerAtkSound.Play();
                    bool hitOrMiss = CheckSuccess(35);
                    if (hitOrMiss == true)
                    {
                        
                        int critDmg;
                        Random flamethrowerAtk = new Random();
                        compFlamethrowerAtk = flamethrowerAtk.Next(25, 41);
                        didFlamethrowerCrit = CriticalHit(compFlamethrowerAtk);

                        if (didCrunchCrit == true)
                        {
                            critDmg = (compFlamethrowerAtk * 2);
                            currentUserHP = currentUserHP - critDmg;

                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            if (critDmg == 0)
                            {
                                MessageBox.Show("The Monsters Flamethrower ability missed! ");
                            }
                            else
                            {
                                MessageBox.Show("Flamethrower has Crit for " + critDmg + " Damage!");
                            }
                        }
                        else
                        {
                            critDmg = compFlamethrowerAtk;
                            currentUserHP = currentUserHP - critDmg;
                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            MessageBox.Show("The Monster attacks you with Flamethrower for " + Convert.ToString(compFlamethrowerAtk) + " Damage!");
                        }

                        
                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;
                        //Change the hp of the comp based on this atk
                        //currentUserHP = currentUserHP - compFlamethrowerAtk;
                        lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
                        uTotalDmgTaken = cTotalDmgDealt;
                        pointDmgTaken = (-1) * (uTotalDmgTaken);
                        //pointDmgTaken + score = score;
                        //lblScore.Text = "Score: " + score;

                        lblCompDmgGiv.Text = ("Monster Damage Dealt: " + Convert.ToString(cTotalDmgDealt));
                        lblUserDmgTaken.Text = ("Player Damage Taken: " + Convert.ToString(uTotalDmgTaken));
                        return compFlamethrowerAtk;
                    }
                    else
                    {
                        compFlamethrowerAtk = 0;
                        MessageBox.Show("The Monsters Flamethrower ability missed! ");
                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;
                        return compFlamethrowerAtk;
                    }
                }
                else
                {
                    hyperBeamAtkSound.Play();
                    bool hitOrMiss = CheckSuccess(15);
                    if (hitOrMiss == true)
                    {
                        int critDmg;
                        
                        Random hyperBeamAtk = new Random();
                        compHyperBeamAtk = hyperBeamAtk.Next(30, 76);
                        didHyperBeamCrit = CriticalHit(compHyperBeamAtk);

                        if (didHyperBeamCrit == true)
                        {
                            critDmg = (compHyperBeamAtk * 2);
                            currentUserHP = currentUserHP - critDmg;

                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            if (critDmg == 0)
                            {
                                MessageBox.Show("The Monsters Hyper Beam ability missed! ");
                            }
                            else
                            {
                                MessageBox.Show("Hyper Beam has Crit for " + critDmg + " Damage!");
                            }
                        }
                        else
                        {
                            critDmg = compHyperBeamAtk;
                            currentUserHP = currentUserHP - critDmg;
                            cTotalDmgDealt = cTotalDmgDealt + critDmg;
                            MessageBox.Show("The Monster attacks you with Hyper Beam for " + Convert.ToString(compHyperBeamAtk) + " Damage!");
                        }


                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;
                        //Change the hp of the comp based on this atk
                        //currentUserHP = currentUserHP - compHyperBeamAtk;
                        lblUserHealth.Text = "HP " + currentUserHP + " / " + userHealth;
                        uTotalDmgTaken = cTotalDmgDealt;
                        pointDmgTaken = (-1) * (uTotalDmgTaken);
                        //pointDmgTaken + score = score;
                        //lblScore.Text = "Score: " + score;

                        lblCompDmgGiv.Text = ("Monster Damage Dealt: " + Convert.ToString(cTotalDmgDealt));
                        lblUserDmgTaken.Text = ("Player Damage Taken: " + Convert.ToString(uTotalDmgTaken));
                        return compHyperBeamAtk;
                    }
                    else
                    {
                        compHyperBeamAtk = 0;
                        MessageBox.Show("The Monsters Hyper Beam ability missed! ");
                        turnSystem = true;
                        btnStab.Enabled = true;
                        btnRupture.Enabled = true;
                        btnShuriken.Enabled = true;
                        btnHeal.Enabled = true;
                        return compHyperBeamAtk;
                    }
                }

            }

        }
        
        //Picks who is first
        private bool FirstTurn()
        {        
            Random generator = new Random();
            
            randomTurnGen = generator.Next(1, 3);
            if (randomTurnGen == 1)
            {
                turn = "The Player will go first!";
                lblWhosTurn.Text = turn;
                return true;
            }
            else
            {
                turn = "The Monster will go first!";
                lblWhosTurn.Text = turn;
                return false;
            }
                                   
        }
        //Checks if the attack has hit based on the hit % chance
        private bool CheckSuccess(int hitPercent)
        {
            Random generator = new Random();
            int hit = generator.Next(1, 101);

            if (hit < hitPercent)
            {
                //result = "You Have Hit!";
                lblUserHOM.Text = "You Have Hit!";
                return true;
            }
            else
            {
                //result = "You Have Missed!";
                lblUserHOM.Text = "You Have Missed!";
                return false;
            }
        }

        //Crit Gen
        private bool CriticalHit(int abilityName)
        {
            Random generator = new Random();
            int num = generator.Next(1, 101);

            if (num < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Hover over button color change (Stab)
        private void btnStab_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnStab.BackColor = Color.RoyalBlue;
            pnlStab.Visible = true;
            pnlRupture.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnStab_MouseLeave(object sender, EventArgs e)
        {
            btnStab.BackColor = Color.Navy;
            pnlStab.Visible = false;
        }

        //(Rupture)
        private void btnRupture_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnRupture.BackColor = Color.RoyalBlue;
            pnlRupture.Visible = true;
            pnlStab.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnRupture_MouseLeave(object sender, EventArgs e)
        {
            btnRupture.BackColor = Color.Navy;
            pnlRupture.Visible = false;
        }

        //(Shuriken)
        private void btnShuriken_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnShuriken.BackColor = Color.RoyalBlue;
            pnlShuriken.Visible = true;
            pnlRupture.Visible = false;
            pnlStab.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnShuriken_MouseLeave(object sender, EventArgs e)
        {
            btnShuriken.BackColor = Color.Navy;
            pnlShuriken.Visible = false;
        }
        //(Heal)
        private void btnHeal_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnHeal.BackColor = Color.RoyalBlue;
            pnlHeal.Visible = true;
            pnlRupture.Visible = false;
            pnlStab.Visible = false;
            pnlShuriken.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnHeal_MouseLeave(object sender, EventArgs e)
        {
            btnHeal.BackColor = Color.Navy;
            pnlHeal.Visible = false;
        }

        private void btnCrunch_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnCrunch.BackColor = Color.RoyalBlue;
            pnlCrunch.Visible = true;
            pnlStab.Visible = false;
            pnlRupture.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnCrunch_MouseLeave(object sender, EventArgs e)
        {
            btnCrunch.BackColor = Color.Navy;
            pnlCrunch.Visible = false;
        }

        private void btnFlamethrower_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnFlamethrower.BackColor = Color.RoyalBlue;
            pnlFlamethrower.Visible = true;
            pnlStab.Visible = false;
            pnlRupture.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlHyperBeam.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnFlamethrower_MouseLeave(object sender, EventArgs e)
        {
            btnFlamethrower.BackColor = Color.Navy;
            pnlFlamethrower.Visible = false;
        }

        private void btnHyperBeam_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnHyperBeam.BackColor = Color.RoyalBlue;
            pnlHyperBeam.Visible = true;
            pnlStab.Visible = false;
            pnlRupture.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlRegen.Visible = false;
        }

        private void btnHyperBeam_MouseLeave(object sender, EventArgs e)
        {
            btnHyperBeam.BackColor = Color.Navy;
            pnlHyperBeam.Visible = false;
        }

        private void btnRegen_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnRegen.BackColor = Color.RoyalBlue;
            pnlRegen.Visible = true;
            pnlStab.Visible = false;
            pnlRupture.Visible = false;
            pnlShuriken.Visible = false;
            pnlHeal.Visible = false;
            pnlCrunch.Visible = false;
            pnlFlamethrower.Visible = false;
            pnlHyperBeam.Visible = false;
        }

        private void btnRegen_MouseLeave(object sender, EventArgs e)
        {
            btnRegen.BackColor = Color.Navy;
            pnlRegen.Visible = false;
        }

        private void btnStartGame_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnStartGame.BackColor = Color.LightSteelBlue;
        }

        private void btnStartGame_MouseLeave(object sender, EventArgs e)
        {
            btnStartGame.BackColor = Color.White;
        }

        private void btnHelp_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnHelp.BackColor = Color.LightSteelBlue;
        }

        private void btnHelp_MouseLeave(object sender, EventArgs e)
        {
            btnHelp.BackColor = Color.White;
        }

        private void btnReset_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnReset.BackColor = Color.Navy;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.BackColor = Color.Black;
        }

        private void btnHelpPlay_MouseHover(object sender, EventArgs e)
        {
            cursorHover.Play();
            btnHelpPlay.BackColor = Color.Navy;
        }

        private void btnHelpPlay_MouseLeave(object sender, EventArgs e)
        {
            btnHelpPlay.BackColor = Color.Black;
        }

        private void scoreUpdater_Tick(object sender, EventArgs e)
        {
            score = pointDmgDealt + pointHeals + pointDmgTaken;
            
    
            lblScore.Text = "Score: " + Convert.ToString(score);
            if (currentUserHP <= 0)
            {
                scoreUpdater.Enabled = false;
                MessageBox.Show("You have been Defeated , You have Lost the Game.");
                ResetGame();
            }
            if (currentCompHP <= 0)
            {
                scoreUpdater.Enabled = false;
                MessageBox.Show("You have Defeated the Monster, You have Won!");
                ResetGame();
            }
        }

        private void btnCrunch_Click(object sender, EventArgs e)
        {
            cursorError.Play();
        }

        private void btnFlamethrower_Click(object sender, EventArgs e)
        {
            cursorError.Play();
        }

        private void btnHyperBeam_Click(object sender, EventArgs e)
        {
            cursorError.Play();
        }

        private void btnRegen_Click(object sender, EventArgs e)
        {
            cursorError.Play();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            pnlPlay.Visible = false;
            cTotalDmgDealt = 0;
            uTotalDmgDealt = 0;
            cTotalDmgTaken = 0;
            uTotalDmgTaken = 0;
            pointDmgDealt = 0;
            pointHeals = 0;
            pointDmgTaken = 0;
            scoreUpdater.Enabled = false;
            score = 0;
            lblScore.Text = "Score: " + Convert.ToString(score);
            lblUserDmgGiv.Text = ("Player Damage Dealt: " + Convert.ToString(uTotalDmgDealt));
            lblCompDmgTaken.Text = ("Monster Damage Taken: " + Convert.ToString(cTotalDmgTaken));
            lblCompDmgGiv.Text = ("Player Damage Dealt: " + Convert.ToString(cTotalDmgDealt));
            lblUserDmgTaken.Text = ("Monster Damage Taken: " + Convert.ToString(uTotalDmgTaken));
        }

        private void frmBattleGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Finds all processes by the name of PlayingSounds
            Process[] processes = Process.GetProcessesByName("PlayingSounds");
            //Kills the process to end the endless music playing problem
            foreach (Process process in processes)
            {
               process.Kill();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlHelp.Visible = false;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            pnlHelp.Visible = true;
        }

        private void btnHelpPlay_Click(object sender, EventArgs e)
        {
            pnlHelp.Visible = true;
        }
    }
}
