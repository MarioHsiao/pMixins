﻿//----------------------------------------------------------------------- 
// <copyright file="Repository.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, July 11, 2014 12:44:37 PM</date> 
// Licensed under the Apache License, Version 2.0,
// you may not use this file except in compliance with this License.
//  
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an 'AS IS' BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright> 
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopaceticSoftware.pMixins.Attributes;
using pMixins.AutoGenerated.pMixins.Mvc.Recipes.Repository.MyEntityRepository.pMixins.Mvc.Recipes.Repository.SqlCreate;
using pMixins.AutoGenerated.pMixins.Mvc.Recipes.Repository.MyEntityRepository.pMixins.Mvc.Recipes.Repository.SqlReadById;
using pMixins.Mvc.Recipes.NonPublicNonParameterlessConstructor;

namespace pMixins.Mvc.Recipes.Repository
{
    public interface IEntity
    {
        int Id { get; }
    }


    public interface IReadById<out TEntity>
        where TEntity : IEntity
    {
        TEntity ReadById(int id);
    }

    public interface IReadAll<out TEntity>
    {
        IEnumerable<TEntity> ReadAll();
    }

    public interface ICreate<in TEntity>
    {
        bool Create(TEntity entity);
    }

    public interface IUpdate<in TEntity>
    {
        bool Update(TEntity entity);
    }

    public interface IDelete<in TEntity>
    {
        bool Delete(TEntity entity);
    }

    public class MyEntity : IEntity
    {
        public int Id { get; private set; }
    }

    public abstract class RepositoryBase
    {
        protected abstract string ConnectionString { get; }

        private void ExecuteQuery(string cmdTxt, SqlParameter[] parameters, Action<SqlCommand> query)
        {
            //error handling
            //transaction coordinating
            //logging

            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var sqlCommand = new SqlCommand(cmdTxt, sqlConnection))
            {
                //Initialize sqlCommand
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (var p in parameters)
                    sqlCommand.Parameters.Add(p);

                //Execute Query
                query(sqlCommand);
            }
        }

        protected void ExecuteNonQuery(string spName, SqlParameter[] parameters)
        {
            
            this.ExecuteQuery(spName, parameters, cmd => cmd.ExecuteNonQuery());
        }

        protected void ExecuteReader(string spName, SqlParameter[] parameters, Action<SqlDataReader> readerCallback)
        {
            this.ExecuteQuery(spName, parameters, cmd => readerCallback(cmd.ExecuteReader()));
        }

        protected object ExecuteScalar(string spName, SqlParameter[] parameters)
        {
            object returnValue = null;

            this.ExecuteQuery(spName, parameters, cmd => returnValue = cmd.ExecuteScalar());

            return returnValue;
        }
    }

    public abstract class SqlReadById<TEntity> : RepositoryBase, IReadById<TEntity>
        where TEntity : IEntity
    {
        protected abstract string ReadByIdStoredProcedureName { get; set; }

        protected abstract TEntity MapEntity(SqlDataReader reader);

        public virtual TEntity ReadById(int id)
        {
            TEntity returnValue = default(TEntity);

            base.ExecuteReader(
                ReadByIdStoredProcedureName,
                new[] {new SqlParameter("id", id)},
                reader => returnValue = MapEntity(reader));

            return returnValue;
        }
    }

    public abstract class SqlCreate<TEntity> : RepositoryBase, ICreate<TEntity>
        where TEntity : IEntity
    {
        protected abstract string CreateStoredProcedureName { get; set; }

        protected abstract SqlParameter[] MapEntityToSqlParameters(TEntity entity);

        public virtual bool Create(TEntity entity)
        {
            base.ExecuteNonQuery(
                CreateStoredProcedureName,
                MapEntityToSqlParameters(entity));

            return true;
        }
    }

    [pMixin(Mixin = typeof(SqlReadById<MyEntity>))]
    [pMixin(Mixin = typeof(SqlCreate<MyEntity>))]
    public partial class MyEntityRepository
    {
        string ISqlReadById__MyEntity__Requirements.ConnectionStringImplementation
        {
            get { throw new NotImplementedException(); }
        }

        SqlParameter[] ISqlCreate__MyEntity__Requirements.MapEntityToSqlParametersImplementation(MyEntity entity)
        {
            throw new NotImplementedException();
        }

        string ISqlCreate__MyEntity__Requirements.CreateStoredProcedureNameImplementation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        MyEntity ISqlReadById__MyEntity__Requirements.MapEntityImplementation(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        string ISqlReadById__MyEntity__Requirements.ReadByIdStoredProcedureNameImplementation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string ISqlCreate__MyEntity__Requirements.ConnectionStringImplementation
        {
            get { throw new NotImplementedException(); }
        }
    }
}
