﻿//
//  IPackage.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Warcraft.MPQ.FileInfo;

namespace Warcraft.MPQ
{
    /// <summary>
    /// Interface for file-providing packages. A package must be able to provide a file list and output data
    /// for files stored in it.
    /// </summary>
    [PublicAPI]
    public interface IPackage
    {
        /// <summary>
        /// Attempts to extract the file at <paramref name="filePath"/> from the package.
        /// </summary>
        /// <param name="filePath">The full path to the file in the package.</param>
        /// <param name="data">The bytes contained in the file.</param>
        /// <returns>true if the file was successfully extracted; Otherwise, false.</returns>
        [PublicAPI]
        [ContractAnnotation("=> false, data : null; => true, data : notnull")]
        bool TryExtractFile(string filePath, [NotNullWhen(true)] out byte[]? data);

        /// <summary>
        /// Determines whether this package has a file list.
        /// </summary>
        /// <returns><c>true</c> if this package has a listfile; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        bool HasFileList();

        /// <summary>
        /// Gets the best available file list from the package. If an external file list has been provided,
        /// that one is prioritized over the one stored in the archive.
        /// </summary>
        /// <returns>The file list.</returns>
        [PublicAPI]
        IEnumerable<string> GetFileList();

        /// <summary>
        /// Checks if the specified file exists in the package.
        /// </summary>
        /// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
        /// <param name="filePath">The full path to the file.</param>
        [PublicAPI]
        bool ContainsFile(string filePath);

        /// <summary>
        /// Attempts to retrieve the file info of the provided path.
        /// </summary>
        /// <param name="filePath">The full path to the file.</param>
        /// <param name="fileInfo">The file info.</param>
        /// <returns>true if the file info was successfully retrieved; otherwise, false.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file is not present in the package.</exception>
        [PublicAPI]
        [ContractAnnotation("=> false, fileInfo : null; => true, fileInfo : notnull")]
        bool TryGetFileInfo(string filePath, [NotNullWhen(true)] out MPQFileInfo? fileInfo);
    }
}
