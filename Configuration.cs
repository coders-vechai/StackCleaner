﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace StackCleaner;

/// <summary>
/// Used to configure properties of the StackCleaner including color and behavior flags.
/// </summary>
public class StackCleanerConfiguration : ICloneable
{
    private const string FrozenErrorText = "Configuration is frozen.";
    private static StackCleanerConfiguration? _default;
    internal bool Frozen;
    private IFormatProvider _locale = CultureInfo.InvariantCulture;
    private ColorConfig _colors = Color4Config.Default;
    private bool _includeSourceData = true;
    private bool _putSourceDataOnNewLine = true;
    private bool _includeNamespaces = true;
    private bool _includeLineData = true;
    private bool _useTypeAliases = true;
    private bool _htmlWriteOuterDiv = true;
    private bool _warnForHiddenLines;
    private bool _htmlUseClassNames;
    private bool _includeILOffset;
    private bool _includeFileData;
    private StackColorFormatType _colorFormatting = StackColorFormatType.None;
    private IReadOnlyCollection<Type> _hiddenTypes = StackTraceCleaner.DefaultHiddenTypes;

    /// <summary>
    /// Default implementation of <see cref="StackCleanerConfiguration"/>.
    /// </summary>
    public static StackCleanerConfiguration Default => _default ??= new StackCleanerConfiguration { Frozen = true };

    /// <summary>
    /// Instance of <see cref="Color4Config"/>, <see cref="Color32Config"/>, or override <see cref="ColorConfig"/> and make your own color provider.<br/>
    /// Default value is <see cref="Color4Config.Default"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public ColorConfig? Colors
    {
        get => _colors;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _colors = value ?? Color4Config.Default;
            if (_colors is Color4Config && _colorFormatting == StackColorFormatType.ExtendedANSIColor)
                _colorFormatting = StackColorFormatType.ANSIColor;
        }
    }

    /// <summary>
    /// Used to convert line numbers, column numbers, and IL offsets to strings.<br/>
    /// Default value is <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public IFormatProvider Locale
    {
        get => _locale;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _locale = value ?? CultureInfo.InvariantCulture;
        }
    }

    /// <summary>
    /// Source data includes line number, column number, IL offset, and file name when available.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool IncludeSourceData
    {
        get => _includeSourceData;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _includeSourceData = value;
        }
    }

    /// <summary>
    /// Appends a warning to the end of the stack trace that lines were removed for visibility.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool WarnForHiddenLines
    {
        get => _warnForHiddenLines;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _warnForHiddenLines = value;
        }
    }

    /// <summary>
    /// Source data is put on the line after the stack frame declaration.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool PutSourceDataOnNewLine
    {
        get => _putSourceDataOnNewLine;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _putSourceDataOnNewLine = value;
        }
    }

    /// <summary>
    /// Namespaces are included in the method declaration.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool IncludeNamespaces
    {
        get => _includeNamespaces;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _includeNamespaces = value;
        }
    }

    /// <summary>
    /// IL offsets are included in the source data.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool IncludeILOffset
    {
        get => _includeILOffset;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _includeILOffset = value;
        }
    }

    /// <summary>
    /// Line and column numbers are included in the source data.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool IncludeLineData
    {
        get => _includeLineData;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _includeLineData = value;
        }
    }

    /// <summary>
    /// Relative source file path will be included in the source data.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool IncludeFileData
    {
        get => _includeFileData;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _includeFileData = value;
        }
    }

    /// <summary>
    /// Primitive types will use their aliases.
    /// </summary>
    /// <remarks>ex. <see cref="int"/> instead of <see cref="Int32">Int32</see>.</remarks>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool UseTypeAliases
    {
        get => _useTypeAliases;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _useTypeAliases = value;
        }
    }

    /// <summary>
    /// If <see cref="ColorFormatting"/> is set to <see cref="StackColorFormatType.Html"/>,
    /// use css class names (defined as public constants in <see cref="StackTraceCleaner"/>) instead of style tags.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool HtmlUseClassNames
    {
        get => _htmlUseClassNames;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _htmlUseClassNames = value;
        }
    }

    /// <summary>
    /// If <see cref="ColorFormatting"/> is set to <see cref="StackColorFormatType.Html"/>,
    /// write an outer &lt;div&gt; with a background color around the output HTML.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public bool HtmlWriteOuterDiv
    {
        get => _htmlWriteOuterDiv;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _htmlWriteOuterDiv = value;
        }
    }

    /// <summary>
    /// Describes how colors will be handled by the formatter.
    /// </summary>
    /// <remarks>Default value is <see cref="StackColorFormatType.ConsoleColor"/>.</remarks>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    /// <exception cref="ArgumentOutOfRangeException">Value is out of range of <see cref="StackColorFormatType"/>.</exception>
    public StackColorFormatType ColorFormatting
    {
        get => _colorFormatting;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            if (value is < 0 or > StackColorFormatType.Html)
                throw new ArgumentOutOfRangeException(nameof(value));
            _colorFormatting = value;
        }
    }

    /// <summary>
    /// Override types who's methods will be skipped in stack traces.<br/>
    /// Default values:<br/><br/>
    /// <see cref="ExecutionContext"/><br/>
    /// <see cref="TaskAwaiter"/><br/>
    /// <see cref="TaskAwaiter{TResult}"/><br/>
    /// <see cref="ConfiguredTaskAwaitable.ConfiguredTaskAwaiter"/><br/>
    /// <see cref="ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter"/><br/>
    /// <see cref="ExceptionDispatchInfo"/><br/>
    /// Use <see cref="GetHiddenTypes"/> to <see langword="get"/> the value.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    /// <remarks>Setting a <see langword="null"/> value will be converted to an empty array.</remarks>
    public ICollection<Type>? HiddenTypes
    {
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            if (value == null)
                value = Array.AsReadOnly(Array.Empty<Type>());
            else if (!ReferenceEquals(value, StackTraceCleaner.DefaultHiddenTypes))
            {
                if (value is Type[] arr)
                {
                    Type[] arr2 = new Type[arr.Length];
                    Array.Copy(arr, arr2, arr.Length);
                    value = Array.AsReadOnly(arr2);
                }
                else
                    value = Array.AsReadOnly(value.ToArray());
            }

            _hiddenTypes = (IReadOnlyCollection<Type>)value;
        }
    }

    /// <summary>As close as possible to Visual Studio default formatting.</summary>
    public StackCleanerConfiguration() { }

    /// <summary>
    /// Creates a non-frozen clone of this <see cref="StackCleanerConfiguration"/>.
    /// </summary>
    /// <returns>An exact copy of this <see cref="StackCleanerConfiguration"/>, safe to cast. Never is frozen.</returns>
    public object Clone() => new StackCleanerConfiguration
    {
        _colorFormatting = _colorFormatting,
        _colors = _colors,
        _hiddenTypes = _hiddenTypes,
        _includeSourceData = _includeSourceData,
        _putSourceDataOnNewLine = _putSourceDataOnNewLine,
        _useTypeAliases = _useTypeAliases,
        _includeNamespaces = _includeNamespaces
    };

    /// <returns>A readonly array representing the current hidden types. May equal <see cref="StackTraceCleaner.DefaultHiddenTypes"/> </returns>
    public IReadOnlyCollection<Type> GetHiddenTypes() => _hiddenTypes;
}

/// <summary>
/// Describes the color formatting behavior of <see cref="StackTraceCleaner"/>
/// </summary>
public enum StackColorFormatType
{
    /// <summary>
    /// No color formatting, just raw text.
    /// </summary>
    None,
    /// <summary>
    /// Sets the <see cref="Console.ForegroundColor"/> for each section. Only applicable when printed to console.
    /// </summary>
    ConsoleColor,
    /// <summary>
    /// UnityEngine rich text tags.<br/>
    /// <seealso href="https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html"/>
    /// </summary>
    UnityRichText,
    /// <summary>
    /// TextMeshPro rich text tags.<br/>
    /// <seealso href="http://digitalnativestudios.com/textmeshpro/docs/rich-text/"/>
    /// </summary>
    TextMeshProRichText,
    /// <summary>
    /// ANSI Text formatting codes.<br/>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences?redirectedfrom=MSDN#text-formatting"/>
    /// </summary>
    ANSIColor,
    /// <summary>
    /// Will not work on all terminals.<br/>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences?redirectedfrom=MSDN#extended-colors"/>
    /// </summary>
    ExtendedANSIColor,
    /// <summary>
    /// Text is colored with &lt;span&gt; tags.
    /// </summary>
    /// <remarks>Use classes instead of constant CSS styles by setting <see cref="StackCleanerConfiguration.HtmlUseClassNames"/> to <see langword="true"/>.</remarks>
    Html
}


/// <summary>
/// Base class for all color configurations.<br/>
/// To use your own source of colors, override this class and set the values of the color fields to a 32 bit ARGB integer.
/// <code>
/// Alpha = (argb &gt;&gt; 24) &amp; 0xFF
/// Red   = (argb &gt;&gt; 16) &amp; 0xFF
/// Green = (argb &gt;&gt; 8 ) &amp; 0xFF
/// Blue  = (argb &gt;&gt; 0 ) &amp; 0xFF
/// </code>
/// </summary>
public abstract class ColorConfig
{
    /// <summary>Error text used to throw a <see cref="NotSupportedException"/> when the config is frozen.</summary>
    protected const string FrozenErrorText = "Color configuration is frozen.";
    private int _keywordColor;
    private int _methodColor;
    private int _propertyColor;
    private int _parameterColor;
    private int _classColor;
    private int _structColor;
    private int _flowKeywordColor;
    private int _interfaceColor;
    private int _genericParameterColor;
    private int _enumColor;
    private int _namespaceColor;
    private int _punctuationColor;
    private int _extraDataColor;
    private int _linesHiddenWarningColor;
    private int _htmlBackgroundColor;
    /// <summary>
    /// This property is set to <see langword="true"/> after the config is passed to a <see cref="StackTraceCleaner"/> so it can not be modified.
    /// </summary>
    public bool Frozen { get; internal set; }

    /// <summary>
    /// Color of keywords including types: <br/><see langword="null"/>, <see langword="bool"/>,
    /// <see langword="byte"/>, <see langword="char"/>, <see langword="double"/>,
    /// <see langword="decimal"/>, <see langword="float"/>, <see langword="int"/>, <see langword="long"/>,
    /// <see langword="sbyte"/>, <see langword="short"/>, <see langword="object"/>, <see langword="string"/>,
    /// <see langword="uint"/>, <see langword="bool"/>, <see langword="ulong"/>, <see langword="ushort"/>,
    /// <see langword="void"/><br/>
    /// method keywords: <br/><see langword="static"/>, <see langword="async"/>, <see langword="enumerator"/>,
    /// <see langword="get"/>, <see langword="set"/>, <see langword="anonymous"/><br/>
    /// and the <see langword="global"/> and <see langword="params"/> keywords.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int KeywordColor
    {
        get => _keywordColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _keywordColor = value;
        }
    }

    /// <summary>
    /// Color of method names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int MethodColor
    {
        get => _methodColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _methodColor = value;
        }
    }

    /// <summary>
    /// Color of property names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int PropertyColor
    {
        get => _propertyColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _propertyColor = value;
        }
    }

    /// <summary>
    /// Color of parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int ParameterColor
    {
        get => _parameterColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _parameterColor = value;
        }
    }

    /// <summary>
    /// Color of class type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int ClassColor
    {
        get => _classColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _classColor = value;
        }
    }

    /// <summary>
    /// Color of value type names (not including enums).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int StructColor
    {
        get => _structColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _structColor = value;
        }
    }

    /// <summary>
    /// Color of flow keywords, currently only used for the 'at' at the beginning of each declaration.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int FlowKeywordColor
    {
        get => _flowKeywordColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _flowKeywordColor = value;
        }
    }

    /// <summary>
    /// Color of interface type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int InterfaceColor
    {
        get => _interfaceColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _interfaceColor = value;
        }
    }

    /// <summary>
    /// Color of generic parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int GenericParameterColor
    {
        get => _genericParameterColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _genericParameterColor = value;
        }
    }

    /// <summary>
    /// Color of enum type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int EnumColor
    {
        get => _enumColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _enumColor = value;
        }
    }

    /// <summary>
    /// Color of namespaces.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int NamespaceColor
    {
        get => _namespaceColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _namespaceColor = value;
        }
    }

    /// <summary>
    /// Color of any punctuation: periods, commas, parenthesis, etc.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int PunctuationColor
    {
        get => _punctuationColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _punctuationColor = value;
        }
    }

    /// <summary>
    /// Color of the source data (line number, column number, IL offset, and file name).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int ExtraDataColor
    {
        get => _extraDataColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _extraDataColor = value;
        }
    }

    /// <summary>
    /// Color of the warning optionally shown when unnecessary lines are removed.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int LinesHiddenWarningColor
    {
        get => _linesHiddenWarningColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _linesHiddenWarningColor = value;
        }
    }

    /// <summary>
    /// Color of the optionally added background when writing as HTML.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public virtual int HtmlBackgroundColor
    {
        get => _htmlBackgroundColor;
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            _htmlBackgroundColor = value;
        }
    }

    internal static int ToArgb(ConsoleColor color)
    {
        // Based off Windows 10 Console colors from https://en.wikipedia.org/wiki/ANSI_escape_code#3-bit_and_4-bit
        return color switch
        {
            ConsoleColor.Black => unchecked((int)0xff0c0c0c),
            ConsoleColor.DarkRed => unchecked((int)0xffc50f1f),
            ConsoleColor.DarkGreen => unchecked((int)0xff13a10e),
            ConsoleColor.DarkYellow => unchecked((int)0xffc19c00),
            ConsoleColor.DarkBlue => unchecked((int)0xff0037da),
            ConsoleColor.DarkMagenta => unchecked((int)0xff881798),
            ConsoleColor.DarkCyan => unchecked((int)0xff3a96dd),
            ConsoleColor.DarkGray => unchecked((int)0xff767676),
            ConsoleColor.Red => unchecked((int)0xffe74856),
            ConsoleColor.Green => unchecked((int)0xff16c60c),
            ConsoleColor.Yellow => unchecked((int)0xfff9f1a5),
            ConsoleColor.Blue => unchecked((int)0xff3b78ff),
            ConsoleColor.Magenta => unchecked((int)0xffb4009e),
            ConsoleColor.Cyan => unchecked((int)0xff61d6d6),
            ConsoleColor.White => unchecked((int)0xfff2f2f2),
            _ => unchecked((int)0xffcccccc) // ConsoleColor.Gray
        };
    }
}

/// <summary>
/// Provides colors in the <see cref="ConsoleColor"/> format.
/// </summary>
public sealed class Color4Config : ColorConfig
{
    private static Color4Config? _default;

    /// <summary>
    /// Default values of <see cref="Color4Config"/>. Frozen.
    /// </summary>
    public static Color4Config Default => _default ??= new Color4Config { Frozen = true };
    /// <summary>
    /// Color of keywords including types: <br/><see langword="null"/>, <see langword="bool"/>,
    /// <see langword="byte"/>, <see langword="char"/>, <see langword="double"/>,
    /// <see langword="decimal"/>, <see langword="float"/>, <see langword="int"/>, <see langword="long"/>,
    /// <see langword="sbyte"/>, <see langword="short"/>, <see langword="object"/>, <see langword="string"/>,
    /// <see langword="uint"/>, <see langword="bool"/>, <see langword="ulong"/>, <see langword="ushort"/>,
    /// <see langword="void"/><br/>
    /// method keywords: <br/><see langword="static"/>, <see langword="async"/>, <see langword="enumerator"/>,
    /// <see langword="get"/>, <see langword="set"/>, <see langword="anonymous"/><br/>
    /// and the <see langword="global"/> and <see langword="params"/> keywords.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor KeywordColor
    {
        get => (ConsoleColor)(base.KeywordColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.KeywordColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of method names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor MethodColor
    {
        get => (ConsoleColor)(base.MethodColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.MethodColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of property names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor PropertyColor
    {
        get => (ConsoleColor)(base.PropertyColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.PropertyColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor ParameterColor
    {
        get => (ConsoleColor)(base.ParameterColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ParameterColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of class type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor ClassColor
    {
        get => (ConsoleColor)(base.ClassColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ClassColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of value type names (not including enums).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor StructColor
    {
        get => (ConsoleColor)(base.StructColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.StructColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of flow keywords, currently only used for the 'at' at the beginning of each declaration.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor FlowKeywordColor
    {
        get => (ConsoleColor)(base.FlowKeywordColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.FlowKeywordColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of interface type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor InterfaceColor
    {
        get => (ConsoleColor)(base.InterfaceColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.InterfaceColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of generic parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor GenericParameterColor
    {
        get => (ConsoleColor)(base.GenericParameterColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.GenericParameterColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of enum type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor EnumColor
    {
        get => (ConsoleColor)(base.EnumColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.EnumColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of namespaces.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor NamespaceColor
    {
        get => (ConsoleColor)(base.NamespaceColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.NamespaceColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of any punctuation: periods, commas, parenthesis, etc.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor PunctuationColor
    {
        get => (ConsoleColor)(base.PunctuationColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.PunctuationColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of the source data (line number, column number, IL offset, and file name).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor ExtraDataColor
    {
        get => (ConsoleColor)(base.ExtraDataColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ExtraDataColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of the warning optionally shown when unnecessary lines are removed.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor LinesHiddenWarningColor
    {
        get => (ConsoleColor)(base.LinesHiddenWarningColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.LinesHiddenWarningColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Color of the optionally added background when writing as HTML.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new ConsoleColor HtmlBackgroundColor
    {
        get => (ConsoleColor)(base.HtmlBackgroundColor - 1);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.HtmlBackgroundColor = (int)value + 1;
        }
    }

    /// <summary>
    /// Sets the default values for <see cref="Color4Config"/>.
    /// </summary>
    public Color4Config()
    {
        KeywordColor = ConsoleColor.Blue;
        MethodColor = ConsoleColor.DarkYellow;
        PropertyColor = ConsoleColor.White;
        ParameterColor = ConsoleColor.Cyan;
        ClassColor = ConsoleColor.DarkGreen;
        StructColor = ConsoleColor.Green;
        FlowKeywordColor = ConsoleColor.Magenta;
        InterfaceColor = ConsoleColor.Yellow;
        GenericParameterColor = ConsoleColor.Yellow;
        EnumColor = ConsoleColor.Yellow;
        NamespaceColor = ConsoleColor.Gray;
        PunctuationColor = ConsoleColor.DarkGray;
        ExtraDataColor = ConsoleColor.DarkGray;
        LinesHiddenWarningColor = ConsoleColor.Yellow;
        HtmlBackgroundColor = ConsoleColor.Black;
    }

    /// <summary>
    /// Convert ARGB data to <see cref="ConsoleColor"/> (picks the closest).
    /// </summary>
    internal static ConsoleColor ToConsoleColor(int argb)
    {
        int bits = ((argb >> 16) & byte.MaxValue) > 128 || ((argb >> 8) & byte.MaxValue) > 128 || (argb & byte.MaxValue) > 128 ? 8 : 0;
        if (((argb >> 16) & byte.MaxValue) > 180)
            bits |= 4;
        if (((argb >> 8) & byte.MaxValue) > 180)
            bits |= 2;
        if ((argb & byte.MaxValue) > 180)
            bits |= 1;
        return (ConsoleColor)bits;
    }
}

/// <summary>
/// Provides colors in the <see cref="System.Drawing.Color"/> format.
/// </summary>
public sealed class Color32Config : ColorConfig
{
    private static Color32Config? _default;

    /// <summary>
    /// Default values of <see cref="Color32Config"/>. Frozen.
    /// </summary>
    public static Color32Config Default => _default ??= new Color32Config { Frozen = true };
    /// <summary>
    /// Color of keywords including types: <br/><see langword="null"/>, <see langword="bool"/>,
    /// <see langword="byte"/>, <see langword="char"/>, <see langword="double"/>,
    /// <see langword="decimal"/>, <see langword="float"/>, <see langword="int"/>, <see langword="long"/>,
    /// <see langword="sbyte"/>, <see langword="short"/>, <see langword="object"/>, <see langword="string"/>,
    /// <see langword="uint"/>, <see langword="bool"/>, <see langword="ulong"/>, <see langword="ushort"/>,
    /// <see langword="void"/><br/>
    /// method keywords: <br/><see langword="static"/>, <see langword="async"/>, <see langword="enumerator"/>,
    /// <see langword="get"/>, <see langword="set"/>, <see langword="anonymous"/><br/>
    /// and the <see langword="global"/> and <see langword="params"/> keywords.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color KeywordColor
    {
        get => Color.FromArgb(base.KeywordColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.KeywordColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of method names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color MethodColor
    {
        get => Color.FromArgb(base.MethodColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.MethodColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of property names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color PropertyColor
    {
        get => Color.FromArgb(base.PropertyColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.PropertyColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color ParameterColor
    {
        get => Color.FromArgb(base.ParameterColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ParameterColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of class type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color ClassColor
    {
        get => Color.FromArgb(base.ClassColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ClassColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of value type names (not including enums).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color StructColor
    {
        get => Color.FromArgb(base.StructColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.StructColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of flow keywords, currently only used for the 'at' at the beginning of each declaration.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color FlowKeywordColor
    {
        get => Color.FromArgb(base.FlowKeywordColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.FlowKeywordColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of interface type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color InterfaceColor
    {
        get => Color.FromArgb(base.InterfaceColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.InterfaceColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of generic parameter names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color GenericParameterColor
    {
        get => Color.FromArgb(base.GenericParameterColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.GenericParameterColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of enum type names.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color EnumColor
    {
        get => Color.FromArgb(base.EnumColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.EnumColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of namespaces.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color NamespaceColor
    {
        get => Color.FromArgb(base.NamespaceColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.NamespaceColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of any punctuation: periods, commas, parenthesis, etc.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color PunctuationColor
    {
        get => Color.FromArgb(base.PunctuationColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.PunctuationColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of the source data (line number, column number, IL offset, and file name).
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color ExtraDataColor
    {
        get => Color.FromArgb(base.ExtraDataColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.ExtraDataColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of the warning optionally shown when unnecessary lines are removed.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color LinesHiddenWarningColor
    {
        get => Color.FromArgb(base.LinesHiddenWarningColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.LinesHiddenWarningColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Color of the optionally added background when writing as HTML.
    /// </summary>
    /// <exception cref="NotSupportedException">Object is frozen (has been given to a <see cref="StackTraceCleaner"/>).</exception>
    public new Color HtmlBackgroundColor
    {
        get => Color.FromArgb(base.HtmlBackgroundColor);
        set
        {
            if (Frozen)
                throw new NotSupportedException(FrozenErrorText);
            base.HtmlBackgroundColor = value.ToArgb();
        }
    }

    /// <summary>
    /// Sets the default values for <see cref="Color32Config"/>.
    /// </summary>
    public Color32Config()
    {
        KeywordColor = Color.FromArgb(255, 86, 156, 214);
        MethodColor = Color.FromArgb(255, 220, 220, 170);
        PropertyColor = Color.FromArgb(255, 220, 220, 220);
        ParameterColor = Color.FromArgb(255, 156, 220, 254);
        ClassColor = Color.FromArgb(255, 78, 201, 176);
        StructColor = Color.FromArgb(255, 134, 198, 145);
        FlowKeywordColor = Color.FromArgb(255, 216, 160, 223);
        InterfaceColor = Color.FromArgb(255, 184, 215, 163);
        GenericParameterColor = Color.FromArgb(255, 184, 215, 163);
        EnumColor = Color.FromArgb(255, 184, 215, 163);
        NamespaceColor = Color.FromArgb(255, 220, 220, 220);
        PunctuationColor = Color.FromArgb(255, 180, 180, 180);
        ExtraDataColor = Color.FromArgb(255, 98, 98, 98);
        LinesHiddenWarningColor = Color.FromArgb(255, 220, 220, 0);
        HtmlBackgroundColor = Color.FromArgb(255, 30, 30, 30);
    }

    /// <summary>
    /// Convert <see cref="ConsoleColor"/> to <see cref="Color"/>.
    /// </summary>
    public static Color ToColor(ConsoleColor color) => Color.FromArgb(ToArgb(color));

    /// <summary>
    /// Convert ARGB data to <see cref="Color"/>.
    /// </summary>
    public static ConsoleColor ToConsoleColor(Color color) => Color4Config.ToConsoleColor(color.ToArgb());
}