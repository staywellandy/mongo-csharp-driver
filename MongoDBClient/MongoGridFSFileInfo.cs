﻿/* Copyright 2010 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MongoDB.BsonLibrary;

namespace MongoDB.MongoDBClient {
    // this class is patterned after .NET's FileInfo
    public class MongoGridFSFileInfo {
        #region private fields
        private MongoGridFS gridFS;
        private int chunkSize;
        private BsonObjectId id;
        private int length;
        private string md5;
        private string name;
        private DateTime uploadDate;
        #endregion

        #region constructors
        public MongoGridFSFileInfo(
            MongoGridFS gridFS,
            BsonDocument fileInfoDocument
        ) {
            this.gridFS = gridFS;
            chunkSize = fileInfoDocument.GetInt32("chunkSize");
            id = fileInfoDocument.GetObjectId("_id");
            length = fileInfoDocument.GetInt32("length");
            md5 = fileInfoDocument.GetString("md5");
            name = fileInfoDocument.GetString("filename");
            uploadDate = fileInfoDocument.GetDateTime("uploadDate");
        }

        public MongoGridFSFileInfo(
            MongoGridFS gridFS,
            string remoteFileName
        ) {
            this.gridFS = gridFS;
            this.chunkSize = gridFS.Settings.DefaultChunkSize;
            this.id = BsonObjectId.GenerateNewId();
            this.name = remoteFileName;
        }

        public MongoGridFSFileInfo(
            MongoGridFS gridFS,
            string remoteFileName,
            int chunkSize
        ) {
            this.gridFS = gridFS;
            this.chunkSize = chunkSize;
            this.id = BsonObjectId.GenerateNewId();
            this.name = remoteFileName;
        }
        #endregion

        #region public properties
        public int ChunkSize {
            get { return chunkSize; }
        }

        public MongoGridFS GridFS {
            get { return gridFS; }
        }

        public BsonObjectId Id {
            get { return id; }
        }

        public int Length {
            get { return length; }
        }

        public string MD5 {
            get { return md5; }
        }

        public string Name {
            get { return name; }
        }

        public DateTime UploadDate {
            get { return uploadDate; }
        }
        #endregion

        #region public methods
        public StreamWriter AppendText() {
            Stream stream = Open(FileMode.Append, FileAccess.Write);
            return new StreamWriter(stream, Encoding.UTF8);
        }

        public MongoGridFSFileInfo CopyTo(
            string destFileName
        ) {
            throw new NotImplementedException();
        }

        public MongoGridFSFileInfo CopyTo(
            string destFileName,
            bool overwrite
        ) {
            throw new NotImplementedException();
        }

        public MongoGridFSStream Create() {
            throw new NotImplementedException();
        }

        public StreamWriter CreateText() {
            Stream stream = Open(FileMode.Create, FileAccess.Write);
            return new StreamWriter(stream, Encoding.UTF8);
        }

        public void Delete() {
            gridFS.Delete(id);
        }

        public void MoveTo(
            string destFileName
        ) {
            throw new NotImplementedException();
        }

        public MongoGridFSStream Open(
            FileMode fileMode
        ) {
            throw new NotImplementedException();
        }

        public MongoGridFSStream Open(
            FileMode fileMode,
            FileAccess fileAccess
        ) {
            throw new NotImplementedException();
        }

        public MongoGridFSStream OpenRead() {
            return Open(FileMode.Open, FileAccess.Read);
        }

        public MongoGridFSStream OpenWrite() {
            return Open(FileMode.OpenOrCreate, FileAccess.Write);
        }

        public void Refresh() {
            var refreshed = gridFS.FindOne(id);
            if (refreshed == null) {
                string errorMessage = string.Format("GridFS file no longer exists: {0}", id);
                throw new MongoException(errorMessage);
            }

            this.chunkSize = refreshed.chunkSize;
            this.length = refreshed.length;
            this.md5 = refreshed.md5;
            this.name = refreshed.name;
            this.uploadDate = refreshed.uploadDate;
        }
        #endregion
    }
}