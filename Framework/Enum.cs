using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.Enum
{
    // ----------- GraphViewer - Undo Work Type -----------
    public enum UndoRedoWorkType
    {
        GraphCreate, GraphDelete, GraphMove, GraphLinked
    }

    // ----------- GraphViewer - Graph Type -----------
    public enum GraphType
    {
        Scene, Branch, Merge, Script
    }

    // ----------- Game - Game State -----------
    public enum GameState
    {
        MainMenu, InGame, SaveLoad, Option
    }

    // ----------- Game - Speaker Animation Type -----------
    public enum AnimationState
    {
        Idle, SlideInLtoR, SlideInRtoL, SlideInBtoT, SlideInTtoB, SlideOutLtoR, SlideOutRtoL, SlideOutBtoT, SlideOutTtoB, FadeIn, FadeOut, Vibrate, Surprised
    }

    // ----------- Game - Speaker Illustration Position -----------
    public enum SpeakerPos
    {
        Left, Center, Right
    }

    // ----------- Game - Save Slot Manager State -----------
    public enum SaveLoadSlotManagerState
    {
        NewGameSave, OptionSave, Load
    }

    // ----------- Game - Game Interface State -----------
    public enum InGameInterfaceDisplayState
    {
        Dialogue, Branch, EscapeMenu
    }

    // ----------- Game - InGame Interface State -----------
    public enum InGameProgressState
    {
        Idle, Auto, Skip
    }

    // ----------- Game - Dialogue Animation Speed State -----------
    public enum DialogueAnimationSpeed
    {
        Fast, Normal, Slow
    }

    // ----------- Game - InGame UI Type -----------
    public enum InGameUserInterfaceType
    {
        SpeakerName, Dialogue, DialogueButton, BranchButton
    }

    // ----------- Game - Option Tab State -----------
    public enum OptionTabState
    {
        Graphic, Sound, Game
    }

    // -------------------------------------------------

    public class EnumManager
    {

        ////////////////////////////////////////////////////////////////////////
        /// Variables

        ////////////////////////////////////
        /// Singleton
        private static EnumManager _instance = null;


        ////////////////////////////////////////////////////////////////////////
        /// Func

        ////////////////////////////////////
        /// Singleton
        public static EnumManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnumManager();

                    if (_instance == null)
                    {
                        ErrorMessages.Instance.MissingSingletoneObject("EnumManager");
                    }
                }

                return _instance;
            }
        }


        ////////////////////////////////////
        /// Enum Getter to string

        /// <summary>
        /// <para>Eng. Return animtion state enum converted string array. </para>
        /// <para>Kor. 애니메이션 상태 Enum 을 String 배열로 변환하여 반환합니다. </para>
        /// </summary>
        public string[] GetAnimStates()
        {
            string[] _animStates = new string[Constants.SPEAKER_ANIM_COUNT];

            for (int i = 0; i < Constants.SPEAKER_ANIM_COUNT; i++)
            {
                _animStates[i] = ((Enum.AnimationState)i).ToString();
            }

            return _animStates;
        }

        /// <summary>
        /// <para>Eng. Return speaker position enum converted string array. </para>
        /// <para>Kor. 화자 위치 Enum 을 String 배열로 변환하여 반환합니다. </para>
        /// </summary>
        public string[] GetSpeakerPositions()
        {
            string[] _speakerPos = new string[Constants.SPEAKER_POS_COUNT];

            for (int i = 0; i < Constants.SPEAKER_POS_COUNT; i++)
            {
                _speakerPos[i] = ((Enum.SpeakerPos)i).ToString();
            }

            return _speakerPos;
        }

        ////////////////////////////////////
        /// Converter

        /// <summary>
        /// <para>Eng. Convert inputted string to <T> enum data. </para>
        /// <para>Kor. 입력받은 특정 문자열을 <T>형 Enum 데이터로 변환합니다. </para>
        /// </summary>
        public T ToEnum<T>(string _value)
        {
            if (!System.Enum.IsDefined(typeof(T), _value))
            {
                return default;
            }

            return (T)System.Enum.Parse(typeof(T), _value, true);
        }
    }
}