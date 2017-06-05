using System;
using System.Globalization;
using UnityEngine;

//TODO insert this in...
//Needs to be included because conversion to an int doesn't work any other way as far as I can tell,
//and apparently it can't perform ENUM == ENUM or ENUM == int in generic functions
//If you can solve, please feel free to update the Unify page for this class

//TODO T class example(can use with missions)
namespace Assets.Utils
{
    public class EnumeratedDelegate<ENUM, DelegateType>
        where ENUM : struct, IConvertible, IComparable, IFormattable
        where DelegateType : class
    {
        private ENUM m_eCurrentMode;            //The current enum value
        private DelegateType m_fnCurrentFunction;   //The current function being called
        private string[] m_arrNames;                //The names of the enum values
        private DelegateType[] m_arrFunctions;          //The functions corresponding to the enum values
        public int Count
        {
            get { return m_arrNames.Length; }
        }
        public DelegateType CurrentFunction //Get the current function
        {
            get { return m_fnCurrentFunction; }
        }
        public DelegateType[] Functions     //Set the functions safely
        {
            get { return m_arrFunctions; }
            set
            {
                if (m_arrNames == null || m_arrNames.Length == 0)
                    throw new UnityException("Must set enum first!");
                if (value.Length == 0)
                    throw new UnityException("Empty arrays not allowed!");
                if (value.Length != m_arrNames.Length)
                    throw new UnityException("Must have same number of functions as enum values!");
                m_arrFunctions = value;
            }
        }
        public ENUM CurrentMode     //Get and set the current mode. When setting, change the function delegate appropriately
        {
            get { return m_eCurrentMode; }
            set
            {
                //Make sure that the value being passed in even corresponds to an existing enum value
                if (!Enum.IsDefined(typeof(ENUM), value))
                    throw new UnityException("'" + value + "' is not a valid member of the enum '" + typeof(ENUM).Name + "'!");
                for (int i = 0; i < Count; i++)
                {
                    //TODO: Check to see if we should try and set the index by name instead and avoid the CultureInfo
                    //print(value.ToString());
                    //Must convert because conversion to an int doesn't work any other way as far as I can tell,
                    //and apparently it can't perform ENUM == ENUM or ENUM == int in generic functions
                    int nValue = value.ToInt32(new CultureInfo("en-us"));
                    if (i == nValue)
                    {
                        SetFunctionByIndex(i);
                        m_eCurrentMode = value;
                        return;
                    }
                }
                throw new UnityException("Couldn't find the enum somehow!");
            }
        }
        static EnumeratedDelegate()
        {
            //Assert that this class's first generic type is an enum
            if (!typeof(ENUM).IsEnum)
                throw new UnityException("First argument of EnumeratedDelegate must be an enum!");
            //Assert that this class's second generic type is a delegate
            //if (!(typeof(DelegateType).Name.Equals(typeof(Delegate).Name)))
            //	throw new UnityException("Second argument of EnumeratedDelegate must be a Delegate!");
        }
        public EnumeratedDelegate()
        {
            m_arrNames = Enum.GetNames(typeof(ENUM));
            CurrentMode = (ENUM)Enum.GetValues(typeof(ENUM)).GetValue(0);
        }
        public EnumeratedDelegate(int intialState)
        {
            m_arrNames = Enum.GetNames(typeof(ENUM));
            CurrentMode = (ENUM)Enum.ToObject(typeof(ENUM), intialState);
        }
        public EnumeratedDelegate(DelegateType[] arrFuncs)
        {
            m_arrNames = Enum.GetNames(typeof(ENUM));
            Functions = arrFuncs;
            CurrentMode = (ENUM)Enum.GetValues(typeof(ENUM)).GetValue(0);
        }
        public EnumeratedDelegate(DelegateType[] arrFuncs, int intialState)
        {
            m_arrNames = Enum.GetNames(typeof(ENUM));
            Functions = arrFuncs;
            CurrentMode = (ENUM)Enum.ToObject(typeof(ENUM), intialState);
        }

        private void SetFunctionByIndex(int index)
        {
            Debug.Log("Setting function to '" + ((m_arrFunctions[index]) as Delegate).Method + "'");
            m_fnCurrentFunction = m_arrFunctions[index];
        }

        private void SetFunctionByName(string name)
        {
            int i = 0;
            foreach (string _name in m_arrNames)
            {
                if (_name == name)
                {
                    Debug.Log("Setting function to '" + name + "'");
                    m_fnCurrentFunction = m_arrFunctions[i];
                    return;
                }
                i++;
            }
            throw new UnityException("Couldn't find an enum GameState representing given name: " + name);
        }
    }
}