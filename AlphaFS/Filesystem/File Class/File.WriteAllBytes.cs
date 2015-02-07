/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;
using FileStream = System.IO.FileStream;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region WriteAllBytes

      /// <summary>
      ///   Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is
      ///   overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes)
      {
         WriteAllBytesInternal(null, path, bytes, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(null, path, bytes, pathFormat);
      }

      #region Transactional

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes)
      {
         WriteAllBytesInternal(transaction, path, bytes, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(transaction, path, bytes, pathFormat);
      }

      #endregion // Transacted

      #endregion // WriteAllBytes

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method WriteAllBytesInternal() to create a new file as part of a transaction, writes the specified byte array to
      ///   the file, and then closes the file. If the target file already exists, it is overwritten.
      /// </summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      internal static void WriteAllBytesInternal(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         if (bytes == null)
            throw new ArgumentNullException("bytes");

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Create, FileAccess.Write, FileShare.Read, ExtendedFileAttributes.None, null, null, pathFormat))
            fs.Write(bytes, 0, bytes.Length);
      }

      #endregion // WriteAllBytesInternal
   }
}
