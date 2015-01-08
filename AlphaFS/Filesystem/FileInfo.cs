/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   ///   Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the
   ///   creation of <see cref="FileStream"/> objects. This class cannot be inherited.
   /// </summary>
   [SerializableAttribute]
   public sealed class FileInfo : FileSystemInfo
   {
      #region Constructors

      #region FileInfo

      #region .NET

      /// <summary>Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName) : this(null, fileName, PathFormat.Relative)
      {
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName, PathFormat pathFormat) : this(null, fileName, pathFormat)
      {
      }

      #region Transacted

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName)
         : this(transaction, fileName, PathFormat.Relative)
      {
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName, PathFormat pathFormat)
      {
         InitializeInternal(false, transaction, fileName, pathFormat);

         _name = Path.GetFileName(Path.RemoveDirectorySeparator(fileName, false), pathFormat != PathFormat.LongFullPath);            
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // FileInfo

      #endregion // Constructors
      
      #region Methods

      #region .NET

      #region AppendText

      #region .NET

      /// <summary>
      ///   Creates a <see cref="System.IO.StreamWriter"/> that appends text to the file represented by this instance of the
      ///   <see cref="FileInfo"/>.
      /// </summary>
      /// <returns>A new <see cref="StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText()
      {
         return File.AppendTextInternal(Transaction, LongFullName, NativeMethods.DefaultFileEncoding, PathFormat.LongFullPath);
      }

      /// <summary>
      ///   Creates a <see cref="StreamWriter"/> that appends text to the file represented by this instance of the <see cref="FileInfo"/>.
      /// </summary>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <returns>A new <see cref="StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText(Encoding encoding)
      {
         return File.AppendTextInternal(Transaction, LongFullName, encoding, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // AppendText

      #region CopyTo

      #region .NET

      /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
      /// <returns>Returns a new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, PathFormat.Relative);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <see langword="true"/>.</para>
      ///   <para>If the file exists and <paramref name="overwrite"/> is <see langword="false"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><see langword="true"/> to allow an existing file to be overwritten; otherwise, <see langword="false"/>.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, PathFormat.Relative);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
      /// <returns>Returns a new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <see langword="true"/>.</para>
      ///   <para>If the file exists and <paramref name="overwrite"/> is <see langword="false"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><see langword="true"/> to allow an existing file to be overwritten; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }


      
      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, PathFormat.Relative);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, PathFormat.Relative);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.</summary>
      /// <returns>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>
      ///   <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, PathFormat.Relative);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.</summary>
      /// <returns>
      ///   <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>
      ///   <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, PathFormat.Relative);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>
      ///   <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      ///   <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // AlphaFS

      #endregion // CopyTo

      #region Create

      #region .NET

      /// <summary>Creates a file.</summary>
      /// <returns><see cref="FileStream"/>A new file.</returns>
      [SecurityCritical]
      public FileStream Create()
      {
         return File.CreateFileStreamInternal(Transaction, LongFullName, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // Create

      #region CreateText

      #region .NET

      /// <summary>Creates a <see crefe="StreamWriter"/> instance that writes a new text file.</summary>
      /// <returns>A new <see cref="StreamWriter"/></returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamWriter CreateText()
      {
         return new StreamWriter(File.CreateFileStreamInternal(Transaction, LongFullName, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, PathFormat.LongFullPath), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #endregion // CreateText

      #region Decrypt

      #region .NET

      /// <summary>Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>      
      [SecurityCritical]
      public void Decrypt()
      {
         File.EncryptDecryptFileInternal(false, LongFullName, false, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // Decrypt

      #region Delete

      #region .NET

      /// <summary>Permanently deletes a file.</summary>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      ///
      /// <exception cref="IOException">.</exception>
      public override void Delete()
      {
         File.DeleteFileInternal(Transaction, LongFullName, false, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Permanently deletes a file.</summary>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>      
      public void Delete(bool ignoreReadOnly)
      {
         File.DeleteFileInternal(Transaction, LongFullName, ignoreReadOnly, PathFormat.LongFullPath);
      }

      #endregion // AlphaFS

      #endregion // Delete

      #region Encrypt

      #region .NET

      /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>      
      [SecurityCritical]
      public void Encrypt()
      {
         File.EncryptDecryptFileInternal(false, LongFullName, true, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // Encrypt

      #region GetAccessControl

      #region .NET

      /// <summary>
      ///   Gets a <see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the access control list (ACL) entries for
      ///   the file described by the current <see cref="FileInfo"/> object.
      /// </summary>
      /// <returns>
      ///   <see cref="System.Security.AccessControl.FileSecurity"/>A FileSecurity object that encapsulates the access control rules for the
      ///   current file.
      /// </returns>      
      [SecurityCritical]
      public FileSecurity GetAccessControl()
      {
         return File.GetAccessControlInternal<FileSecurity>(false, LongFullName, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.LongFullPath);
      }

      /// <summary>
      ///   Gets a <see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list
      ///   (ACL) entries for the file described by the current FileInfo object.
      /// </summary>
      /// <param name="includeSections">
      ///   One of the <see cref="System.Security"/> values that specifies which group of access control entries to retrieve.
      /// </param>
      /// <returns>
      ///   <see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list (ACL)
      ///   entries for the file described by the current FileInfo object.
      /// </returns>      
      [SecurityCritical]
      public FileSecurity GetAccessControl(AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<FileSecurity>(false, LongFullName, includeSections, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // GetAccessControl

      #region MoveTo

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      [SecurityCritical]
      public void MoveTo(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, PathFormat.Relative);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.</summary>
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, MoveOptions moveOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, null, moveOptions, null, null, out destinationPathLp, PathFormat.Relative);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.</summary>
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, null, moveOptions, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, PathFormat.Relative);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.</summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // AlphaFS

      #endregion // MoveTo

      #region Open

      #region .NET

      /// <summary>Opens a file in the specified mode.</summary>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.
      /// </param>
      /// <returns>A <see cref="FileStream"/> file opened in the specified mode, with read/write access and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
      /// <param name="mode">A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
      /// <returns>A <see cref="FileStream"/> object opened in the specified mode and access, and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.
      /// </param>
      /// <param name="access">
      ///   A <see cref="FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.
      /// </param>
      /// <param name="share">
      ///   A <see cref="FileShare"/> constant specifying the type of access other <see cref="FileStream"/> objects have to this file.
      /// </param>
      /// <returns>A <see cref="FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, share, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Opens a file in the specified mode with read, write, or read/write access.</summary>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.
      /// </param>
      /// <param name="rights">
      ///   A <see cref="FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the
      ///   contents of existing files are retained or overwritten along with additional options.
      /// </param>
      /// <returns>A <see cref="FileStream"/> object opened in the specified mode and access, and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileSystemRights rights)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      /// <summary>
      ///   [AlphaFS] Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
      /// </summary>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.
      /// </param>
      /// <param name="rights">
      ///   A <see cref="FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the
      ///   contents of existing files are retained or overwritten along with additional options.
      /// </param>
      /// <param name="share">
      ///   A <see cref="FileShare"/> constant specifying the type of access other <see cref="FileStream"/> objects have to this file.
      /// </param>
      /// <returns>A <see cref="FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileSystemRights rights, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, share, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      #endregion // AlphaFS

      #endregion // Open

      #region OpenRead

      #region .NET

      /// <summary>Creates a read-only <see cref="FileStream"/>.</summary>
      /// <remarks>This method returns a read-only <see cref="FileStream"/> object with the <see cref="FileShare"/> mode set to Read.</remarks>
      /// <returns>A new read-only <see cref="FileStream"/> object.</returns>
      [SecurityCritical]
      public FileStream OpenRead()
      {
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // OpenRead

      #region OpenText

      #region .NET

      /// <summary>
      ///   Creates a <see cref="StreamReader"/> with NativeMethods.DefaultFileEncoding encoding that reads from an existing text file.
      /// </summary>
      /// <returns>A new <see cref="StreamReader"/> with NativeMethods.DefaultFileEncoding encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamReader OpenText()
      {
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a <see cref="StreamReader"/> with <see cref="Encoding"/> that reads from an existing text file.</summary>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A new <see cref="StreamReader"/> with the specified <see cref="Encoding"/>.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamReader OpenText(Encoding encoding)
      {
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath), encoding);
      }

      #endregion // AlphaFS

      #endregion // OpenText

      #region OpenWrite

      #region .NET

      /// <summary>Creates a write-only <see cref="FileStream"/>.</summary>
      /// <returns>A write-only unshared <see cref="FileStream"/> object for a new or existing file.</returns>
      [SecurityCritical]
      public FileStream OpenWrite()
      {
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // OpenWrite

      #region Refresh

      #region .NET

      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }

      #endregion // .NET

      #endregion // Refresh

      #region Replace

      #region .NET

      /// <summary>
      ///   Replaces the contents of a specified file with the file described by the current <see cref="FileInfo"/> object, deleting the
      ///   original file, and creating a backup of the replaced file.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of the file described by the current
      ///   <see cref="FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new
      ///   <see cref="FileInfo"/> object that describes the overwritten file.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">
      ///   The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.
      /// </param>
      /// <returns>
      ///   A <see cref="FileInfo"/> object that encapsulates information about the file described by the
      ///   <paramref name="destinationFileName"/> parameter.
      /// </returns>      
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
      {
         return Replace(destinationFileName, destinationBackupFileName, false, PathFormat.Relative);
      }

      /// <summary>
      ///   Replaces the contents of a specified file with the file described by the current <see cref="FileInfo"/> object, deleting the
      ///   original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of the file described by the current
      ///   <see cref="FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new
      ///   <see cref="FileInfo"/> object that describes the overwritten file.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">
      ///   The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.
      /// </param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file;
      ///   otherwise, <see langword="false"/>.
      /// </param>
      /// <returns>
      ///   A <see cref="FileInfo"/> object that encapsulates information about the file described by the
      ///   <paramref name="destinationFileName"/> parameter.
      /// </returns>
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         return Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors, PathFormat.Relative);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Replaces the contents of a specified file with the file described by the current <see cref="FileInfo"/> object, deleting
      ///   the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of the file described by the current
      ///   <see cref="FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new
      ///   <see cref="FileInfo"/> object that describes the overwritten file.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">
      ///   The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.
      /// </param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file;
      ///   otherwise, <see langword="false"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileInfo"/> object that encapsulates information about the file described by the
      ///   <paramref name="destinationFileName"/> parameter.
      /// </returns>      
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         string destinationFileNameLp = Path.GetExtendedLengthPathInternal(Transaction, destinationFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
         string destinationBackupFileNameLp = Path.GetExtendedLengthPathInternal(Transaction, destinationBackupFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
          
         File.ReplaceInternal(LongFullName, destinationFileNameLp, destinationBackupFileNameLp, ignoreMetadataErrors, PathFormat.LongFullPath);

         return new FileInfo(Transaction, destinationFileNameLp, PathFormat.LongFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Replace

      #region SetAccessControl

      #region .NET

      /// <summary>
      ///   Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.
      /// </summary>
      /// <remarks>
      ///   The SetAccessControl method applies access control list (ACL) entries to the current file that represents the noninherited ACL
      ///   list. Use the SetAccessControl method whenever you need to add or remove ACL entries from a file.
      /// </remarks>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity"/> object that describes an access control list (ACL) entry to apply to the current file.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(FileSecurity fileSecurity)
      {
         File.SetAccessControlInternal(LongFullName, null, fileSecurity, AccessControlSections.All, PathFormat.LongFullPath);
      }

      /// <summary>
      ///   Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.
      /// </summary>
      /// <remarks>
      ///   The SetAccessControl method applies access control list (ACL) entries to the current file that represents the noninherited ACL
      ///   list. Use the SetAccessControl method whenever you need to add or remove ACL entries from a file.
      /// </remarks>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity"/> object that describes an access control list (ACL) entry to apply to the current file.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(FileSecurity fileSecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(LongFullName, null, fileSecurity, includeSections, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #endregion // SetAccessControl

      #region ToString

      #region .NET

      /// <summary>Returns the path as a string.</summary>
      /// <returns>The path.</returns>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET

      #endregion // ToString

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to the file.</summary>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>      
      [SecurityCritical]
      public void AddStream(string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, Transaction, LongFullName, name, contents, PathFormat.LongFullPath);
      }

      #endregion // AddStream

      #region Compress

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>      
      [SecurityCritical]
      public void Compress()
      {
         Device.ToggleCompressionInternal(false, Transaction, LongFullName, true, PathFormat.LongFullPath);
      }

      #endregion // Compress
      
      #region Decompress

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      
      [SecurityCritical]
      public void Decompress()
      {
         Device.ToggleCompressionInternal(false, Transaction, LongFullName, false, PathFormat.LongFullPath);
      }

      #endregion // Decompress

      #region EnumerateStreams

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file.</summary>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file.</returns>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(false, Transaction, null, LongFullName, null, null, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file.</summary>
      /// <param name="streamType">Type of the stream.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file.
      /// </returns>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams(StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(false, Transaction, null, LongFullName, null, streamType, PathFormat.LongFullPath);
      }

      #endregion // EnumerateStreams

      #region GetStreamSize

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all streams.</returns>      
      [SecurityCritical]
      public long GetStreamSize()
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, null, null, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public long GetStreamSize(string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, name, StreamType.Data, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public long GetStreamSize(StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, null, type, PathFormat.LongFullPath);
      }

      #endregion GetStreamSize
      
      #region RefreshEntryInfo

      /// <summary>Refreshes the state of the <see cref="FileSystemEntryInfo"/> EntryInfo instance.</summary>
      [SecurityCritical]
      public new void RefreshEntryInfo()
      {
         base.RefreshEntryInfo();
      }

      #endregion // RefreshEntryInfo

      #region RemoveStream

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from the file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>      
      [SecurityCritical]
      public void RemoveStream()
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, Transaction, LongFullName, null, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from the file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="name">The name of the stream to remove.</param>
      [SecurityCritical]
      public void RemoveStream(string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, Transaction, LongFullName, name, PathFormat.LongFullPath);
      }

      #endregion // RemoveStream


      #region Unified Internals

      #region CopyToMoveToInternal

      /// <summary>[AlphaFS] Unified method CopyToMoveToInternal() to copy/move an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <param name="destinationPath"><para>A full path string to the destination directory</para></param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="copyOptions"><para>This parameter can be <see langword="null"/>. Use <see cref="CopyOptions"/> to specify how the file is to be copied.</para></param>
      /// <param name="moveOptions"><para>This parameter can be <see langword="null"/>. Use <see cref="MoveOptions"/> that specify how the file is to be moved.</para></param>
      /// <param name="progressHandler"><para>This parameter can be <see langword="null"/>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <see langword="null"/>. The argument to be passed to the callback function.</para></param>
      /// <param name="longFullPath">[out] Returns the retrieved long full path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SecurityCritical]
      private CopyMoveResult CopyToMoveToInternal(string destinationPath, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, out string longFullPath, PathFormat pathFormat)
      {
         string destinationPathLp = Path.GetExtendedLengthPathInternal(Transaction, destinationPath, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         longFullPath = destinationPathLp;

         // Returns false when CopyMoveProgressResult is PROGRESS_CANCEL or PROGRESS_STOP.
         return File.CopyMoveInternal(false, Transaction, LongFullName, destinationPathLp, preserveDates, copyOptions, moveOptions, progressHandler, userProgressData, PathFormat.LongFullPath);
      }

      private void CopyToMoveToInternalRefresh(string destinationPath, string destinationPathLp)
      {
         LongFullName = destinationPathLp;
         FullPath = Path.GetRegularPathInternal(destinationPathLp, false, false, false, false);

         OriginalPath = destinationPath;
         DisplayPath = OriginalPath;

         _name = Path.GetFileName(destinationPathLp, true);

         // Flush any cached information about the file.
         Reset();
      }

      #endregion // CopyToMoveToInternal

      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region Directory

      /// <summary>
      ///   Gets an instance of the parent directory.
      /// </summary>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      /// <value>A <see cref="DirectoryInfo"/> object representing the parent directory of this file.</value>
      ///
      /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
      public DirectoryInfo Directory
      {
         get
         {
            string dirName = DirectoryName;
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, PathFormat.FullPath);
         }
      }

      #endregion // Directory

      #region DirectoryName

      /// <summary>
      ///   Gets a string representing the directory's full path.
      /// </summary>
      /// <remarks>
      ///   <para>To get the parent directory as a DirectoryInfo object, use the Directory property.</para>
      ///   <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// <value>A string representing the directory's full path.</value>
      ///
      /// <exception cref="ArgumentNullException">null was passed in for the directory name.</exception>
      public string DirectoryName
      {
         [SecurityCritical] get { return Path.GetDirectoryName(FullPath, false); }
      }

      #endregion // DirectoryName

      #region Exists

      /// <summary>
      ///   Gets a value indicating whether the file exists.
      /// </summary>
      /// <remarks>
      ///   <para>The <see cref="Exists"/> property returns <see langword="false"/> if any error occurs while trying to determine if the
      ///   specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,
      ///   </para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// <value><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</value>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public override bool Exists
      {
         [SecurityCritical]
         get
         {
            try
            {
               if (DataInitialised == -1)
                  Refresh();

               return DataInitialised == 0 && (Win32AttributeData.FileAttributes & FileAttributes.Directory) == 0;
            }
            catch
            {
               return false;
            }
         }
      }

      #endregion // Exists

      #region IsReadOnly

      /// <summary>
      ///   Gets or sets a value that determines if the current file is read only.
      /// </summary>
      /// <remarks>
      ///   <para>Use the IsReadOnly property to quickly determine or change whether the current file is read only.</para>
      ///   <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// <value><see langword="true"/> if the current file is read only; otherwise, <see langword="false"/>.</value>
      ///
      /// <exception cref="FileNotFoundException">The file described by the current FileInfo object could not be found.</exception>
      /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
      public bool IsReadOnly
      {
         get { return (Attributes & FileAttributes.ReadOnly) != 0; }

         set
         {
            if (value)
               Attributes |= FileAttributes.ReadOnly;
            else
               Attributes &= ~FileAttributes.ReadOnly;
         }
      }

      #endregion // IsReadOnly

      #region Length

      /// <summary>
      ///   Gets the size, in bytes, of the current file.
      /// </summary>
      /// <remarks>
      ///   <para>The value of the Length property is pre-cached</para>
      ///   <para>To get the latest value, call the Refresh method.</para>
      /// </remarks>
      /// <value>The size of the current file in bytes.</value>
      ///
      /// <exception cref="System.IO.FileNotFoundException">
      ///   The file does not exist or the Length property is called for a directory.
      /// </exception>
      /// <exception cref="System.IO.IOException">.</exception>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public long Length
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            FileAttributes attrs = Win32AttributeData.FileAttributes;

            // MSDN: .NET 3.5+: FileNotFoundException: The file does not exist or the Length property is called for a directory.
            if (attrs == (FileAttributes) (-1))
               NativeError.ThrowException(Win32Errors.ERROR_FILE_NOT_FOUND, LongFullName);

            // MSDN: .NET 3.5+: FileNotFoundException: The file does not exist or the Length property is called for a directory.
            if ((attrs & FileAttributes.Directory) == FileAttributes.Directory)
               NativeError.ThrowException(Win32Errors.ERROR_FILE_NOT_FOUND, string.Format(CultureInfo.CurrentCulture, Resources.DirectoryExistsWithSameNameSpecifiedByPath, LongFullName));

            return Win32AttributeData.FileSize;
         }
      }

      #endregion // Length

      #region Name

      private string _name;

      /// <summary>
      ///   Gets the name of the file.
      /// </summary>
      /// <remarks>
      ///   <para>The name of the file includes the file extension.</para>
      ///   <para>When first called, <see cref="FileInfo"/> calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      ///   <para>The name of the file includes the file extension.</para>
      /// </remarks>
      /// <value>The name of the file.</value>
      public override string Name
      {
         get { return _name; }
      }

      #endregion // Name

      #endregion // .NET

      #endregion // Properties
   }
}