using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Action = MediaPortal.GUI.Library.Action;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
//using MediaPortal.Playlists;
//using MediaPortal.Util;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
//using Cornerstone.Tools;
//Cornerstone.Database;
//using Cornerstone.Database.Tables;
//using MediaPortal.Plugins.MovingPictures;
//using MediaPortal.Plugins.MovingPictures.Database;
//using MediaPortal.Plugins.MovingPictures.MainUI;
//using WindowPlugins.GUITVSeries;
//using TVSeriesHelper = WindowPlugins.GUITVSeries.Helper;



namespace ProcessPlugins.AvalonEditor
{
  [PluginIcons("AvalonPlugin.Plugin.png", "AvalonPlugin.PluginDisabled.png")]
  public class AvalonPlugin : IPlugin, ISetupForm
    {

        #region declarations
        int mypointer = 0;
        int myprevimage = 0;
        int numberofitems = 0;
        int sel_image = 0;
        //int sel_label = 0;
        int topitem1;
        int topitem2;
        int topitem3;
        int bottomitem1;
        int bottomitem2;
        int bottomitem3;
        string myskin = "";
        //string mousecontrol = "";
        List<string> ms_prefixes = new List<string>();
        List<string> ms_headings = new List<string>();
        List<int> ms_windowids = new List<int>();
        List<int> ms_subids = new List<int>();
        List<string> ms_window_params = new List<string>();
        List<string> ms_sub_params = new List<string>();
        string _hyperLinkParameter = "";
        List<string> ms_bgimages = new List<string>();
        List<string> ms_bgpaths = new List<string>();
        List<string> ms_recentMedia = new List<string>();
        private ArrayList Children = new ArrayList();
        private ArrayList subChildren = new ArrayList();
        bool hasSetNeighbours = false;
        bool pauseMainMenu = false;
        string selectedSubItem = "35";
        int selectedID = 0;
        int location = 0;
        //Timer mytimer = new Timer();
        //public string[] mostTVSeriesRecents = new string[3];
        //public string[] mostMovPicsRecents = new string[3];
        System.Windows.Forms.Timer mrTimer = new System.Windows.Forms.Timer();

        public static bool IsUpdateAvailable { get; set; }

        //latest Series
        bool latestSeries1 = false;
        bool latestSeries2 = false;
        bool latestSeries3 = false;

        int latestSeriesPlay1 = 91919994;
        int latestSeriesPlay2 = 91919995;
        int latestSeriesPlay3 = 91919996;

        //latest Movies
        bool latestMovies1 = false;
        bool latestMovies2 = false;
        bool latestMovies3 = false;

        int latestMoviesPlay1 = 91919991;
        int latestMoviesPlay2 = 91919992;
        int latestMoviesPlay3 = 91919993;

        //latest MyFilms
        bool latestMyFilms1 = false;
        bool latestMyFilms2 = false;
        bool latestMyFilms3 = false;

        int latestMyFilmsPlay1 = 91919988;
        int latestMyFilmsPlay2 = 91919989;
        int latestMyFilmsPlay3 = 91919990;

        //latest Music
        bool latestMusic1 = false;
        bool latestMusic2 = false;
        bool latestMusic3 = false;

        int latestMusicPlay1 = 91919997;
        int latestMusicPlay2 = 91919998;
        int latestMusicPlay3 = 91919999;

        //latest Recordings
        bool latestRecordings1 = false;
        bool latestRecordings2 = false;
        bool latestRecordings3 = false;
        bool latestRecordings4 = false;

        int latestRecordingsPlay1 = 91919984;
        int latestRecordingsPlay2 = 91919985;
        int latestRecordingsPlay3 = 91919986;
        int latestRecordingsPlay4 = 91919987;

        //Mainmenu
        bool mainMenuDown1 = false;
        bool mainMenuDown2 = false;
        bool mainMenuUp1 = false;
        bool mainMenuUp2 = false;

        int mainMenuDownID1 = 801;
        int mainMenuDownID2 = 802;
        int mainMenuUpID1 = 901;
        int mainMenuUpID2 = 902;

        //dummy buttons
        int dummyMovies = 71717771;
        int dummySeries = 71717772;
        int dummyMusic = 71717773;
        int dummyRecordings = 71717774;
        int dummyMyFilms = 71717775;

        #endregion


        public class Child
        {
            public int Owner;
            public int ID;
            public string Name;
            public int Hyperlink;
            public string Parameter;

            public Child(int intOwner, int strID, string strName, int intHyperlink, string strParam)
            {
                this.Owner = intOwner;
                this.ID = strID;
                this.Name = strName;
                this.Hyperlink = intHyperlink;
                this.Parameter = strParam;
            }

            public override string ToString()
            {
                return this.ID + " : " + this.Name;
            }

        }

        #region Private Methods

        private void SetProperty(string property, string value)
        {
            if (property == null)
                return;
            // If the value is empty always add a space
            // otherwise the property will keep 
            // displaying it's previous value
            if (String.IsNullOrEmpty(value))
                value = " ";

            GUIPropertyManager.SetProperty(property, value);


        }

        private void SetBlankProperty(string property, string value)
        {
            if (property == null)
                return;
            // If the value is empty always add a space
            // otherwise the property will keep 
            // displaying it's previous value
            if (String.IsNullOrEmpty(value))
                value = "";

            GUIPropertyManager.SetProperty(property, value);
        }

        private void SetSubmenu(int owner)
        {
            GUIControl.HideControl(35, 566);
            //Log.Info("hidectrl 1");

            for (int i = 0; i < 9; i++)
            {
                SetProperty("#subitem" + i, "");
            }


            int a = 0;
            subChildren.Clear();
            ms_subids.Clear();
            ms_sub_params.Clear();
            foreach (Child mychild in Children)
            {
                if (mychild.Owner == owner)
                {
                    subChildren.Add(mychild);
                    ms_subids.Add(mychild.Hyperlink);
                    ms_sub_params.Add(mychild.Parameter);
                    a++;


                    //GUIControl.SetControlLabel(35, (500 + a), mychild.Name);
                    //GUIControl.SetControlLabel(35, (600 + a), mychild.Name);

                    SetProperty("#subitem" + a, mychild.Name);
                }

            }

            if (a > 0)
            {
                SetProperty("#isarrowvisible", "yes");
                GUIControl.ShowControl(35, 566);
                //Log.Info("showctrl");
            }
            else
            {
                SetProperty("#isarrowvisible", "no");
                GUIControl.HideControl(35, 566);
                //Log.Info("hidectrl 2");
            }
        }

        private void SetNeighbours(int mypointer, int numberofitems)
        {

            //SetProperty("#items", "mypointer:" + mypointer + " number:" + numberofitems);


            if (ms_headings.Count > 0)
            {
                if (mypointer - 2 > 0)
                {
                    topitem3 = mypointer - 3;
                    topitem2 = mypointer - 2;
                    topitem1 = mypointer - 1;
                }

                else if (mypointer - 1 > 0)
                {
                    topitem3 = numberofitems;
                    topitem2 = mypointer - 2;
                    topitem1 = mypointer - 1;
                }
                else if (mypointer > 0)
                {
                    topitem3 = numberofitems - 1;
                    topitem2 = numberofitems;
                    topitem1 = mypointer - 1;
                }
                else if (mypointer - 1 == 0)
                {
                    topitem3 = numberofitems - 1;
                    topitem2 = numberofitems;
                    topitem1 = 0;
                }
                else if (mypointer == 0)
                {
                    topitem3 = numberofitems - 2;
                    topitem2 = numberofitems - 1;
                    topitem1 = numberofitems;
                }







                if (mypointer + 1 > numberofitems)
                {
                    bottomitem1 = 0;
                }
                else
                {
                    bottomitem1 = mypointer + 1;
                }




                if (mypointer == numberofitems)
                {
                    bottomitem2 = 1;
                }

                else if (mypointer + 1 == numberofitems)
                {
                    bottomitem2 = 0;
                }

                else if (mypointer + 1 > numberofitems)
                {
                    bottomitem2 = 1;
                }

                else
                {
                    bottomitem2 = mypointer + 2;
                }



                if (mypointer == numberofitems)
                {
                    bottomitem3 = 2;
                }
                else if (mypointer + 2 == numberofitems)
                {
                    bottomitem3 = 0;
                }
                else if (mypointer + 1 == numberofitems)
                {
                    bottomitem3 = 1;
                }
                else
                {
                    bottomitem3 = mypointer + 3;
                }

                if (ms_headings[topitem3] != null)
                {
                    SetProperty("#topitem3", ms_headings[topitem3]);
                }
                if (ms_headings[topitem2] != null)
                {
                    SetProperty("#topitem2", ms_headings[topitem2]);
                }
                if (ms_headings[topitem1] != null)
                {
                    SetProperty("#topitem1", ms_headings[topitem1]);
                }
                if (ms_headings[bottomitem1] != null)
                {
                    SetProperty("#bottomitem1", ms_headings[bottomitem1]);
                }
                if (ms_headings[bottomitem2] != null)
                {
                    SetProperty("#bottomitem2", ms_headings[bottomitem2]);
                }
                if (hasSetNeighbours == true && ms_headings[bottomitem3] != null)
                {
                    SetProperty("#bottomitem3", ms_headings[bottomitem3]);
                }
                else
                {
                    SetProperty("#bottomitem3", " ");
                }
                hasSetNeighbours = true;
            }

        }

        private void noItems(string myMessage)
        {
            GUIDialogOK dlg = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
            if (myMessage != "")
            {
                dlg.SetHeading(myMessage);
            }
            else
            {
                dlg.SetHeading("No menuitems configured");
            }
            dlg.SetLine(1, "Avalon found no menuitems,");
            dlg.SetLine(2, "please run the config-plugin");
            dlg.SetLine(3, "to generate your BasicHome menu.");
            dlg.DoModal(GUIWindowManager.ActiveWindow);

        }

        private void GUIPropertyManager_OnPropertyChanged(string tag, string tagValue)
        {
            //Log.Info("Maya: tag=" + tag + ", tagValue=" + tagValue);

            if (tag == "#currentmoduleid" && tagValue.IndexOf("35") != -1)
            {
                SetSubmenu(mypointer);
            }
            else if (tag == "#highlightedbutton" && tagValue.IndexOf("#subitem") != -1)
            {
                bool test1 = String.Compare(tagValue.Substring(0, 8), "#subitem") == 0; // This is true.
                selectedSubItem = tagValue.Substring(8);
                int numVal = Convert.ToInt32(selectedSubItem) - 1;

                selectedID = Convert.ToInt32(tagValue.Substring(8)) - 1;
                SetProperty("#highlightedbutton", subChildren[numVal].ToString().Substring(4));
                //Log.Info("Maya : bool=" + test1 + " selected = " + selectedSubItem);

                //Log.Info("Maya: paus hovedmenu");
                pauseMainMenu = true;
            }
            else if (tag == "#homeitem")
            {
                //Log.Info("Maya: genstart hovedmenu");
                //pauseMainMenu = false;
            }
            else if (tag == "#highlightedbutton" && tagValue.ToString() == "")
            {
                //Log.Info("Maya: genstart hovedmenu");
                //pauseMainMenu = false;

            }
        }

        private void hidePoster()
        {
            string isHidePosterEnabled = GUIPropertyManager.GetProperty("#Avalon.EnablePosterHide");

            if (isHidePosterEnabled == "true")
            {
                mrTimer.Enabled = true;
                mrTimer.Interval = 3000;
                mrTimer.Tick += new EventHandler(mrTimer_Tick);
            }
        }

        private void checkPoster()
        {
            SetProperty("#Avalon.hidePoster", "yes");
        }

        private void mrTimer_Tick(object sender, EventArgs e)
        {
            checkPoster();
        }

        private void ActionDown()
        {
            
            bool bReturn1 = false;
            bool bReturn2 = false;
            bool bReturn3 = false;

            bool bReturn10 = false;


            GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            if (vWindow != null)
            {
                // Note: This'll only work for unique id's
                GUIControl control1 = vWindow.GetControl(2);
                GUIControl control2 = vWindow.GetControl(3);
                GUIControl control3 = vWindow.GetControl(4);


                GUIControl control10 = vWindow.GetControl(99999);


                bReturn1 = control1.Focus;
                bReturn2 = control2.Focus;
                bReturn3 = control3.Focus;


                latestRecordings1 = vWindow.GetControl(latestRecordingsPlay1).Focus;
                latestRecordings2 = vWindow.GetControl(latestRecordingsPlay2).Focus;
                latestRecordings3 = vWindow.GetControl(latestRecordingsPlay3).Focus;
                latestRecordings4 = vWindow.GetControl(latestRecordingsPlay4).Focus;

                latestMusic1 = vWindow.GetControl(latestMusicPlay1).Focus;
                latestMusic2 = vWindow.GetControl(latestMusicPlay2).Focus;
                latestMusic3 = vWindow.GetControl(latestMusicPlay3).Focus;

                latestMovies1 = vWindow.GetControl(latestMoviesPlay1).Focus;
                latestMovies2 = vWindow.GetControl(latestMoviesPlay2).Focus;
                latestMovies3 = vWindow.GetControl(latestMoviesPlay3).Focus;

                latestMyFilms1 = vWindow.GetControl(latestMyFilmsPlay1).Focus;
                latestMyFilms2 = vWindow.GetControl(latestMyFilmsPlay2).Focus;
                latestMyFilms3 = vWindow.GetControl(latestMyFilmsPlay3).Focus;

                latestSeries1 = vWindow.GetControl(latestSeriesPlay1).Focus;
                latestSeries2 = vWindow.GetControl(latestSeriesPlay2).Focus;
                latestSeries3 = vWindow.GetControl(latestSeriesPlay3).Focus;

                bReturn10 = control10.Focus;

                if (!bReturn1 && !bReturn2 && !bReturn3 && !latestMovies1 && !latestMovies2 && !latestMovies3 && !latestMyFilms1 && !latestMyFilms2 && !latestMyFilms3 && !latestSeries1 && !latestSeries2 && !latestSeries3 && !bReturn10 && !latestMusic1 && !latestMusic2 && !latestMusic3 && !latestRecordings1 && !latestRecordings2 && !latestRecordings3 && !latestRecordings4)
                {
                    SetProperty("#mainmenu", "");

                    if (mypointer == numberofitems)
                    {
                        myprevimage = mypointer;
                        mypointer = 0;
                    }
                    else
                    {
                        myprevimage = mypointer;
                        mypointer++;
                    }
                    SetNeighbours(mypointer, numberofitems);
                    if (ms_headings.Count > 0)
                    {
                        SetProperty("#homeitem", ms_headings[mypointer]);
                        SetSubmenu(mypointer);
                        GUIControl.SetControlLabel(35, 801, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 802, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 901, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 902, ms_headings[mypointer]);

                        SetProperty("#recentMedia", ms_recentMedia[mypointer]);

                        if (sel_image == 0)
                        {
                            SetBlankProperty("#visbgimage1", "");
                            SetProperty("#visbgimage2", "yes");
                            SetProperty("#bgimage1", ms_bgimages[myprevimage]);
                            SetProperty("#bgimage2", ms_bgimages[mypointer]);
                            sel_image = 1;
                        }
                        else
                        {
                            SetProperty("#visbgimage1", "yes");
                            SetBlankProperty("#visbgimage2", "");
                            SetProperty("#bgimage1", ms_bgimages[mypointer]);
                            SetProperty("#bgimage2", ms_bgimages[myprevimage]);
                            sel_image = 0;
                        }

                        //Log.Info(ms_bgimages[mypointer]);


                    }
                    else
                    {
                        noItems("");
                    }
                    return;

                }
            }
            return;
        }

        private void ActionUp()
        {
            bool bReturn1 = false;
            bool bReturn2 = false;
            bool bReturn3 = false;
            bool bReturn10 = false;


            GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            if (vWindow != null)
            {
                // Note: This'll only work for unique id's
                GUIControl control1 = vWindow.GetControl(2);
                GUIControl control2 = vWindow.GetControl(3);
                GUIControl control3 = vWindow.GetControl(4);

                GUIControl control10 = vWindow.GetControl(99999);


                bReturn1 = control1.Focus;
                bReturn2 = control2.Focus;
                bReturn3 = control3.Focus;

                bReturn10 = control10.Focus;

                latestRecordings1 = vWindow.GetControl(latestRecordingsPlay1).Focus;
                latestRecordings2 = vWindow.GetControl(latestRecordingsPlay2).Focus;
                latestRecordings3 = vWindow.GetControl(latestRecordingsPlay3).Focus;
                latestRecordings4 = vWindow.GetControl(latestRecordingsPlay4).Focus;

                latestMusic1 = vWindow.GetControl(latestMusicPlay1).Focus;
                latestMusic2 = vWindow.GetControl(latestMusicPlay2).Focus;
                latestMusic3 = vWindow.GetControl(latestMusicPlay3).Focus;

                latestMovies1 = vWindow.GetControl(latestMoviesPlay1).Focus;
                latestMovies2 = vWindow.GetControl(latestMoviesPlay2).Focus;
                latestMovies3 = vWindow.GetControl(latestMoviesPlay3).Focus;

                latestMyFilms1 = vWindow.GetControl(latestMyFilmsPlay1).Focus;
                latestMyFilms2 = vWindow.GetControl(latestMyFilmsPlay2).Focus;
                latestMyFilms3 = vWindow.GetControl(latestMyFilmsPlay3).Focus;

                latestSeries1 = vWindow.GetControl(latestSeriesPlay1).Focus;
                latestSeries2 = vWindow.GetControl(latestSeriesPlay2).Focus;
                latestSeries3 = vWindow.GetControl(latestSeriesPlay3).Focus;

                if (!bReturn1 && !bReturn2 && !bReturn3 && !latestMovies1 && !latestMovies2 && !latestMovies3 && !latestMyFilms1 && !latestMyFilms2 && !latestMyFilms3 && !bReturn10 && !latestMusic1 && !latestMusic2 && !latestMusic3 && !latestSeries1 && !latestSeries2 && !latestSeries3 && !latestRecordings1 && !latestRecordings2 && !latestRecordings3 && !latestRecordings4)
                {

                    SetProperty("#mainmenu", "");

                    if (mypointer == 0)
                    {
                        myprevimage = mypointer;
                        mypointer = numberofitems;
                    }
                    else
                    {
                        myprevimage = mypointer;
                        mypointer--;
                    }

                    SetNeighbours(mypointer, numberofitems);
                    if (ms_headings.Count > 0)
                    {
                        SetProperty("#homeitem", ms_headings[mypointer]);
                        SetSubmenu(mypointer);
                        GUIControl.SetControlLabel(35, 801, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 802, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 901, ms_headings[mypointer]);
                        GUIControl.SetControlLabel(35, 902, ms_headings[mypointer]);

                        SetProperty("#recentMedia", ms_recentMedia[mypointer]);

                        if (sel_image == 0)
                        {
                            SetBlankProperty("#visbgimage1", "");
                            SetProperty("#visbgimage2", "yes");
                            SetProperty("#bgimage1", ms_bgimages[myprevimage]);
                            SetProperty("#bgimage2", ms_bgimages[mypointer]);
                            sel_image = 1;
                        }
                        else
                        {
                            SetProperty("#visbgimage1", "yes");
                            SetBlankProperty("#visbgimage2", "");
                            SetProperty("#bgimage1", ms_bgimages[mypointer]);
                            SetProperty("#bgimage2", ms_bgimages[myprevimage]);
                            sel_image = 0;
                        }

                    }
                    else
                    {
                        noItems("");
                    }
                    return;
                }
            }
            return;
        }

        private void ActionRight()
        {
            string loc_recent = ms_recentMedia[mypointer];
            bool bReturn5 = false;
            bool bReturn6 = false;
            bool bReturn7 = false;
            bool bReturn8 = false;
            bool haschildren = false;

            GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            if (vWindow != null)
            {

                mainMenuDown1 = vWindow.GetControl(mainMenuDownID1).Focus;
                mainMenuDown2 = vWindow.GetControl(mainMenuDownID2).Focus;
                mainMenuUp1 = vWindow.GetControl(mainMenuUpID1).Focus;
                mainMenuUp2 = vWindow.GetControl(mainMenuUpID2).Focus;

                GUIControl control5 = vWindow.GetControl(99999);
                GUIControl control6 = vWindow.GetControl(3);

                GUIControl control7 = vWindow.GetControl(501);
                GUIControl control8 = vWindow.GetControl(601);

                bReturn5 = control5.Focus;
                bReturn6 = control6.Focus;

                bReturn7 = control7.Visible;
                bReturn8 = control8.Visible;

                latestRecordings4 = vWindow.GetControl(latestRecordingsPlay4).Focus;

                latestMusic1 = vWindow.GetControl(latestMusicPlay1).Focus;
                latestMusic2 = vWindow.GetControl(latestMusicPlay2).Focus;
                latestMusic3 = vWindow.GetControl(latestMusicPlay3).Focus;

                latestMovies1 = vWindow.GetControl(latestMoviesPlay1).Focus;
                latestMovies2 = vWindow.GetControl(latestMoviesPlay2).Focus;
                latestMovies3 = vWindow.GetControl(latestMoviesPlay3).Focus;

                latestMyFilms1 = vWindow.GetControl(latestMyFilmsPlay1).Focus;
                latestMyFilms2 = vWindow.GetControl(latestMyFilmsPlay2).Focus;
                latestMyFilms3 = vWindow.GetControl(latestMyFilmsPlay3).Focus;

                latestSeries1 = vWindow.GetControl(latestSeriesPlay1).Focus;
                latestSeries2 = vWindow.GetControl(latestSeriesPlay2).Focus;
                latestSeries3 = vWindow.GetControl(latestSeriesPlay3).Focus;


                if (!latestMovies1 && !latestMovies2 && !latestMovies3 && !latestMyFilms1 && !latestMyFilms2 && !latestMyFilms3 && !latestMusic1 && !latestMusic2 && !latestMusic3 && !latestSeries1 && !latestSeries2 && !latestSeries3 && !latestRecordings4)
                {


                    if (mainMenuDown1) SetProperty("#mainmenu", "801");
                    if (mainMenuDown2) SetProperty("#mainmenu", "802");
                    if (mainMenuUp1) SetProperty("#mainmenu", "901");
                    if (mainMenuUp2) SetProperty("#mainmenu", "902");

                    if (mainMenuDown1 | mainMenuDown2 | mainMenuUp1 | mainMenuUp2)
                    {
                        foreach (Child mychild in Children)
                        {
                            if (mychild.Owner == mypointer)
                            {
                                haschildren = true;
                            }

                        }
                        if (!haschildren)
                        {
                            if (loc_recent == "MovingPictures")
                            {
                                GUIControl.FocusControl(35, dummyMovies);
                            }
                            if (loc_recent == "TVSeries")
                            {
                                GUIControl.FocusControl(35, dummySeries);
                            }
                            if (loc_recent == "Music")
                            {
                                GUIControl.FocusControl(35, dummyMusic);
                            }
                            if (loc_recent == "Recordings")
                            {
                                GUIControl.FocusControl(35, dummyRecordings);
                            }
                            if (loc_recent == "MyFilms")
                            {
                                GUIControl.FocusControl(35, dummyMyFilms);
                            }
                        }
                    }

                    if (bReturn7 || bReturn8)
                    {
                        if (loc_recent == "MovingPictures")
                        {
                            GUIControl.FocusControl(35, dummyMovies);
                        }
                        if (loc_recent == "TVSeries")
                        {
                            GUIControl.FocusControl(35, dummySeries);
                        }
                        if (loc_recent == "Music")
                        {
                            GUIControl.FocusControl(35, dummyMusic);
                        }
                        if (loc_recent == "Recordings")
                        {
                            GUIControl.FocusControl(35, dummyRecordings);
                        }
                        if (loc_recent == "MyFilms")
                        {
                            GUIControl.FocusControl(35, dummyMyFilms);
                        }
                    }
                }

                pauseMainMenu = false;
            }
            return;
        }

        private void ActionLeft()
        {
            string IsUpdateAvailable = GUIPropertyManager.GetProperty("#Avalon.UpdateAvailable");

            bool bReturn6 = false;
            bool bReturn7 = false;
            bool bReturn8 = false;
            bool bReturn9 = false;
            bool bReturn10 = false;
            //bool bReturn17 = false;


            GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            if (vWindow != null)
            {
                GUIControl control9 = vWindow.GetControl(99999);
                GUIControl control10 = vWindow.GetControl(3);

                GUIControl control7 = vWindow.GetControl(501);
                GUIControl control8 = vWindow.GetControl(601);

                bReturn7 = control7.Visible;
                bReturn8 = control8.Visible;

                bReturn9 = control9.Focus;
                bReturn10 = control10.Focus;


                //GUIControl control17 = vWindow.GetControl(91919987);

                mainMenuDown1 = vWindow.GetControl(mainMenuDownID1).Focus;
                mainMenuDown2 = vWindow.GetControl(mainMenuDownID2).Focus;
                mainMenuUp1 = vWindow.GetControl(mainMenuUpID1).Focus;
                mainMenuUp2 = vWindow.GetControl(mainMenuUpID2).Focus;


                latestMusic1 = vWindow.GetControl(latestMusicPlay1).Focus;
                latestMusic2 = vWindow.GetControl(latestMusicPlay2).Focus;
                latestMusic3 = vWindow.GetControl(latestMusicPlay3).Focus;

                latestMovies1 = vWindow.GetControl(latestMoviesPlay1).Focus;
                latestMovies2 = vWindow.GetControl(latestMoviesPlay2).Focus;
                latestMovies3 = vWindow.GetControl(latestMoviesPlay3).Focus;

                latestMyFilms1 = vWindow.GetControl(latestMyFilmsPlay1).Focus;
                latestMyFilms2 = vWindow.GetControl(latestMyFilmsPlay2).Focus;
                latestMyFilms3 = vWindow.GetControl(latestMyFilmsPlay3).Focus;

                latestSeries1 = vWindow.GetControl(latestSeriesPlay1).Focus;
                latestSeries2 = vWindow.GetControl(latestSeriesPlay2).Focus;
                latestSeries3 = vWindow.GetControl(latestSeriesPlay3).Focus;
                // bReturn17 = control17.Focus;


                if (!latestMovies1 && !latestMovies2 && !latestMovies3 && !latestMyFilms1 && !latestMyFilms2 && !latestMyFilms3 && !bReturn6 && !bReturn9 && !bReturn10 && !latestMusic1 && !latestMusic2 && !latestMusic3 && !latestSeries1 && !latestSeries2 && !latestSeries3)
                {

                    if (mainMenuDown1) SetProperty("#mainmenu", "801");
                    if (mainMenuDown2) SetProperty("#mainmenu", "802");
                    if (mainMenuUp1) SetProperty("#mainmenu", "901");
                    if (mainMenuUp2) SetProperty("#mainmenu", "902");


                    if (mainMenuDown1 | mainMenuDown2 | mainMenuUp1 | mainMenuUp2)
                    {

                        if (IsUpdateAvailable == "true")
                        {
                            GUIControl.FocusControl(35, 99998);
                        }
                        if (IsUpdateAvailable != "true")
                        {

                            GUIControl.FocusControl(35, 99997);
                        }
                    }

                }

                pauseMainMenu = false;
            }

            if (latestMovies1 || latestMyFilms1 || latestSeries1 || latestMusic1 || bReturn7 || bReturn8)
            {
                string selectedMenu = GUIPropertyManager.GetProperty("#mainmenu");
                if (selectedMenu == "801") GUIControl.FocusControl(35, 801);
                if (selectedMenu == "802") GUIControl.FocusControl(35, 802);
                if (selectedMenu == "901") GUIControl.FocusControl(35, 901);
                if (selectedMenu == "902") GUIControl.FocusControl(35, 902);
            }
        }

        private void ActionEnter()
        {
            if (sel_image == 0)
            {
                SetProperty("#bgimage2", "");
            }
            else
            {
                SetProperty("#bgimage1", "");
            }

            GUIControl.HideControl(35, 566);

            bool aReturn1 = false;
            bool aReturn2 = false;
            bool aReturn3 = false;

            bool aReturn11 = false;

            GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            if (vWindow != null)
            {

                // Note: This'll only work for unique id's
                GUIControl control1 = vWindow.GetControl(2);
                GUIControl control2 = vWindow.GetControl(3);
                GUIControl control3 = vWindow.GetControl(4);
                GUIControl control11 = vWindow.GetControl(99999);


                aReturn1 = control1.Focus;
                aReturn2 = control2.Focus;
                aReturn3 = control3.Focus;
                aReturn11 = control11.Focus;

                latestRecordings1 = vWindow.GetControl(latestRecordingsPlay1).Focus;
                latestRecordings2 = vWindow.GetControl(latestRecordingsPlay2).Focus;
                latestRecordings3 = vWindow.GetControl(latestRecordingsPlay3).Focus;
                latestRecordings4 = vWindow.GetControl(latestRecordingsPlay4).Focus;

                latestMusic1 = vWindow.GetControl(latestMusicPlay1).Focus;
                latestMusic2 = vWindow.GetControl(latestMusicPlay2).Focus;
                latestMusic3 = vWindow.GetControl(latestMusicPlay3).Focus;

                latestMovies1 = vWindow.GetControl(latestMoviesPlay1).Focus;
                latestMovies2 = vWindow.GetControl(latestMoviesPlay2).Focus;
                latestMovies3 = vWindow.GetControl(latestMoviesPlay3).Focus;

                latestMyFilms1 = vWindow.GetControl(latestMyFilmsPlay1).Focus;
                latestMyFilms2 = vWindow.GetControl(latestMyFilmsPlay2).Focus;
                latestMyFilms3 = vWindow.GetControl(latestMyFilmsPlay3).Focus;

                latestSeries1 = vWindow.GetControl(latestSeriesPlay1).Focus;
                latestSeries2 = vWindow.GetControl(latestSeriesPlay2).Focus;
                latestSeries3 = vWindow.GetControl(latestSeriesPlay3).Focus;

                if (!aReturn1 && !aReturn2 && !aReturn3 && !latestMovies1 && !latestMovies2 && !latestMovies3 && !latestMyFilms1 && !latestMyFilms2 && !latestMyFilms3 && !aReturn11 && !latestMusic1 && !latestMusic2 && !latestMusic3 && !latestSeries1 && !latestSeries2 && !latestSeries3 && !latestRecordings1 && !latestRecordings2 && !latestRecordings3 && !latestRecordings4)
                {

                    //Do this if it was the main-menu that called the action
                    if (pauseMainMenu == false)
                    {
                        location = (int)ms_windowids[mypointer];
                        _hyperLinkParameter = ms_window_params[mypointer];

                    }
                    else
                    {
                        location = (int)ms_subids[selectedID];
                        _hyperLinkParameter = ms_sub_params[selectedID];

                    }
                    if (location > -1)
                    {

                        if (_hyperLinkParameter != String.Empty)
                        {

                            GUIWindowManager.ActivateWindow((int)location, _hyperLinkParameter);
                        }
                        else
                        {
                            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, location, 0, null);
                            GUIWindowManager.SendThreadMessage(msg);
                        }
                        pauseMainMenu = false;
                        return;
                    }
                }
            }
        }

        private void ActionHome()
        {
            GUIControl.HideControl(35, 566);
            //Log.Info("Avalon: User went home");
            if (ms_headings.Count > 0)
            {
                SetProperty("#homeitem", ms_headings[mypointer]);
                GUIControl.SetControlLabel(35, 801, ms_headings[mypointer]);
                GUIControl.SetControlLabel(35, 802, ms_headings[mypointer]);
                GUIControl.SetControlLabel(35, 901, ms_headings[mypointer]);
                GUIControl.SetControlLabel(35, 902, ms_headings[mypointer]);
            }
            else
            {
                noItems("");
            }
            //SetSubmenu(mypointer);
            pauseMainMenu = false;
        }

        #endregion

        #region Public Methods

        // Catch actions up, down, enter/play and home/back
        public void AvalonAction(Action Action)
        {

            // Hide Poster
            if (GUIWindowManager.ActiveWindow == 96742 || GUIWindowManager.ActiveWindow == 501 || GUIWindowManager.ActiveWindow == 504 || GUIWindowManager.ActiveWindow == 500 || GUIWindowManager.ActiveWindow == 7986 || GUIWindowManager.ActiveWindow == 9811 || GUIWindowManager.ActiveWindow == 6 || GUIWindowManager.ActiveWindow == 25)
            {
                bool aReturn1 = false;
                GUIWindow vWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
                if (vWindow != null)
                {
                    GUIControl control1 = vWindow.GetControl(50);

                    aReturn1 = control1.Focus;

                    if (aReturn1)
                    {

                        if (Action.wID == Action.ActionType.ACTION_MOVE_UP || Action.wID == Action.ActionType.ACTION_MOVE_DOWN)
                        {
                            mrTimer.Enabled = false;
                            mrTimer.Tick -= new EventHandler(mrTimer_Tick);
                            SetProperty("#Avalon.hidePoster", "no");
                        }
                        if (Action.wID != Action.ActionType.ACTION_MOVE_UP || Action.wID != Action.ActionType.ACTION_MOVE_DOWN)
                        {
                            hidePoster();
                        }
                    }
                }
                return;
            }

            if (GUIWindowManager.ActiveWindow == 35 && GUIWindowManager.RoutedWindow == -1) //BasicHome screen and no dialog

            {

                numberofitems = ms_headings.Count - 1;

                bool haschildren2 = false;
                foreach (Child mychild in Children)
                {
                    if (mychild.Owner == mypointer)
                    {
                        haschildren2 = true;

                    }

                }

                if (haschildren2 == false)
                {
                    GUIControl.HideControl(35, 566);
                }


                switch (Action.wID)
                {
                    case Action.ActionType.ACTION_MOVE_LEFT:
                        ActionLeft();
                        break;
                    case Action.ActionType.ACTION_MOVE_RIGHT:
                        ActionRight();
                        break;
                    case Action.ActionType.ACTION_MOVE_DOWN:
                        if (pauseMainMenu == false)
                        {
                            ActionDown();
                        }
                        break;
                    case Action.ActionType.ACTION_MOVE_UP:
                        if (pauseMainMenu == false)
                        {
                            ActionUp();
                        }
                        break;
                    case Action.ActionType.ACTION_SELECT_ITEM:
                    case Action.ActionType.ACTION_MOUSE_CLICK:
                        ActionEnter();
                        break;
                    case Action.ActionType.ACTION_SWITCH_HOME:
                    case Action.ActionType.ACTION_HOME:
                        ActionHome();
                    break;
                    default:   
                        break;
                }
            }
        }


        public void Start()
        {

            //InitMostRecents();

            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
            {
                myskin = xmlreader.GetValueAsString("skin", "name", "");
                //mousecontrol = xmlreader.GetValueAsString("general", "mousesupport", "");
            }

            // Check to see if Avalon is the selected skin (halt plugin if not)
            if (myskin.IndexOf("Avalon") != -1)
            {
                Log.Info("Avalon BasicHome plugin: Skin found");
                //Skin is Avalon, continue

                // Load settings and set the default item
                if (!File.Exists(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
                {
                    Log.Info("Avalon BasicHome plugin: Config-file Avalon.xml not found.");
                    Stop();
                }
                else
                {

                    using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
                    {
                        string mytest = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName0", "");
                        if (mytest.Length > 0)
                        {





                            int a = 0;
                            var submenus = new Object[250, 3];
                            do
                            {

                                mytest = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName" + a, "");
                                if (mytest != "")
                                {
                                    ms_headings.Add(xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName" + a, ""));
                                    ms_windowids.Add(xmlreader.GetValueAsInt("AvalonBasicHome", "menuItemHyperlink" + a, 0));

                                    if (xmlreader.GetValueAsString("AvalonBasicHome", "menuItemBgImage" + a, "").Contains("jpg") || xmlreader.GetValueAsString("AvalonBasicHome", "menuItemBgImage" + a, "").Contains("png"))
                                    {
                                        ms_bgimages.Add("BasicHomeBG/" + xmlreader.GetValueAsString("AvalonBasicHome", "menuItemBgImage" + a, ""));
                                    }
                                    else
                                    {
                                        ms_bgimages.Add(xmlreader.GetValueAsString("AvalonBasicHome", "menuItemBgImage" + a, ""));
                                    }

                                    ms_window_params.Add(xmlreader.GetValueAsString("AvalonBasicHome", "menuItemParameter" + a, ""));
                                    ms_recentMedia.Add(xmlreader.GetValueAsString("AvalonBasicHome", "menuItemRecentMedia" + a, ""));



                                    int b = 0;
                                    do
                                    {
                                        //Read submenu
                                        if (xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, "") != "")
                                        {
                                            //Log.Info("found some submenus, :" + a + ", " + b + "navn: " + xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, ""));
                                            Children.Add(new Child(a, b, xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, ""), xmlreader.GetValueAsInt("AvalonBasicHomeSubmenu" + a, "submenuItemHyperlink" + b, 0), xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemParameter" + b, "")));
                                        }
                                        b++;
                                    } while (b < 250);




                                }
                                a++;
                            } while (a < 25);


                            SetNeighbours(0, ms_headings.Count - 1);
                            SetSubmenu(0);
                            if (ms_headings.Count > 0)
                            {
                                SetProperty("#bgimage1", ms_bgimages[0]);
                                SetProperty("#bgimage2", ms_bgimages[0]);
                                GUIControl.SetControlLabel(35, 801, ms_headings[0]);
                                GUIControl.SetControlLabel(35, 802, ms_headings[0]);
                                GUIControl.SetControlLabel(35, 901, ms_headings[0]);
                                GUIControl.SetControlLabel(35, 902, ms_headings[0]);
                                SetProperty("#homeitem", ms_headings[0]);
                                SetProperty("#recentMedia", ms_recentMedia[0]);
                            }



                        }
                        else
                        {
                            Log.Info("Avalon plugin: No menuitems configured");
                            Stop();
                        }
                        GUIGraphicsContext.OnNewAction += new OnActionHandler(AvalonAction);
                        GUIPropertyManager.OnPropertyChanged += new GUIPropertyManager.OnPropertyChangedHandler(GUIPropertyManager_OnPropertyChanged);
                    }
                }
            }
        }
        

        public void Stop()
        {
            Log.Info("Avalon plugin: Stopped");
        }

        #endregion

        #region ISetupForm Members

        // Returns the name of the plugin which is shown in the plugin menu
        public string PluginName()
        {
            return "Avalon Editor";
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description()
        {
            return "BasicHome.xml modifier, part of the Avalon skin for MediaPortal (by joostzilla and ncoH).";
        }

        // Returns the author of the plugin which is shown in the plugin menu
        public string Author()
        {
            return "ncoH, pilehave";
        }

        // show the setup dialog
        public void ShowPlugin()
        {
            AvalonConfig config = new AvalonConfig();
            config.ShowDialog();
        }

        // Indicates whether plugin can be enabled/disabled
        public bool CanEnable()
        {
            return true;
        }

        // Get Windows-ID
        public int GetWindowId()
        {
            // WindowID of windowplugin belonging to this setup
            // enter your own unique code
            return -1;
        }

        // Indicates if plugin is enabled by default;
        public bool DefaultEnabled()
        {
            return true;
        }

        // indicates if a plugin has it's own setup screen
        public bool HasSetup()
        {
            return true;
        }

        /// <summary>
        /// If the plugin should have it's own button on the main menu of MediaPortal then it
        /// should return true to this method, otherwise if it should not be on home
        /// it should return false
        /// </summary>
        /// <param name="strButtonText">text the button should have</param>
        /// <param name="strButtonImage">image for the button, or empty for default</param>
        /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
        /// <param name="strPictureImage">subpicture for the button or empty for none</param>
        /// <returns>true : plugin needs it's own button on home
        /// false : plugin does not need it's own button on home</returns>

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
          strButtonText = String.Empty;
          strButtonImage = String.Empty;
          strButtonImageFocus = String.Empty;
          strPictureImage = String.Empty;
          return false;
        }



        #endregion

    }
}