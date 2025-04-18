using Altinn.App.Core.Models;
using Altinn.Platform.Storage.Interface.Models;
using Microsoft.AspNetCore.Http;

namespace Altinn.App.Core.Internal.Data;

/// <summary>
/// Interface for data handling
/// </summary>
public interface IDataClient
{
    /// <summary>
    /// Stores the form model
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="dataToSerialize">The app model to serialize</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="type">The type for serialization</param>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="dataType">The data type to create, must be a valid data type defined in application metadata</param>
    Task<DataElement> InsertFormData<T>(
        T dataToSerialize,
        Guid instanceGuid,
        Type type,
        string org,
        string app,
        int instanceOwnerPartyId,
        string dataType
    )
        where T : notnull;

    /// <summary>
    /// Stores the form
    /// </summary>
    /// <typeparam name="T">The model type</typeparam>
    /// <param name="instance">The instance that the data element belongs to</param>
    /// <param name="dataTypeString">The data type with requirements</param>
    /// <param name="dataToSerialize">The data element instance</param>
    /// <param name="type">The class type describing the data</param>
    /// <returns>The data element metadata</returns>
    Task<DataElement> InsertFormData<T>(Instance instance, string dataTypeString, T dataToSerialize, Type type)
        where T : notnull;

    /// <summary>
    /// updates the form data
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="dataToSerialize">The form data to serialize</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="type">The type for serialization</param>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="dataId">the data id</param>
    //TODO: [Obsolete in v9 in favour of a version that gets the dataType so we can support json and xml]
    Task<DataElement> UpdateData<T>(
        T dataToSerialize,
        Guid instanceGuid,
        Type type,
        string org,
        string app,
        int instanceOwnerPartyId,
        Guid dataId
    )
        where T : notnull;

    /// <summary>
    /// Gets the form data
    /// </summary>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="type">The type for serialization</param>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="dataId">the data id</param>
    Task<object> GetFormData(
        Guid instanceGuid,
        Type type,
        string org,
        string app,
        int instanceOwnerPartyId,
        Guid dataId
    );

    /// <summary>
    /// Gets the data as is.
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataId">the data id</param>
    Task<Stream> GetBinaryData(string org, string app, int instanceOwnerPartyId, Guid instanceGuid, Guid dataId);

    /// <summary>
    /// Similar to GetBinaryData, but returns a HttpResponseMessage instead of a cached stream
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataId">the data id</param>
    /// <returns>The raw HttpResponseMessage from the call to platform</returns>
    Task<byte[]> GetDataBytes(string org, string app, int instanceOwnerPartyId, Guid instanceGuid, Guid dataId);

    /// <summary>
    /// Method that gets metadata on form attachments ordered by attachmentType
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <returns>A list with attachments metadata ordered by attachmentType</returns>
    Task<List<AttachmentList>> GetBinaryDataList(string org, string app, int instanceOwnerPartyId, Guid instanceGuid);

    /// <summary>
    /// Method that removes a form attachments from disk/storage
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataGuid">The attachment id</param>
    [Obsolete("Use method DeleteData with delayed=false instead.", error: true)]
    Task<bool> DeleteBinaryData(string org, string app, int instanceOwnerPartyId, Guid instanceGuid, Guid dataGuid);

    /// <summary>
    /// Method that removes a data elemen from disk/storage immediatly or marks it as deleted.
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataGuid">The attachment id</param>
    /// <param name="delay">A boolean indicating whether or not the delete should be executed immediately or delayed</param>
    Task<bool> DeleteData(
        string org,
        string app,
        int instanceOwnerPartyId,
        Guid instanceGuid,
        Guid dataGuid,
        bool delay
    );

    /// <summary>
    /// Method that saves a form attachments to disk/storage and returns the new data element.
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataType">The data type to create, must be a valid data type defined in application metadata</param>
    /// <param name="request">Http request containing the attachment to be saved</param>
    Task<DataElement> InsertBinaryData(
        string org,
        string app,
        int instanceOwnerPartyId,
        Guid instanceGuid,
        string dataType,
        HttpRequest request
    );

    /// <summary>
    /// Method that updates a form attachments to disk/storage and returns the updated data element.
    /// </summary>
    /// <param name="org">Unique identifier of the organisation responsible for the app.</param>
    /// <param name="app">Application identifier which is unique within an organisation.</param>
    /// <param name="instanceOwnerPartyId">The instance owner id</param>
    /// <param name="instanceGuid">The instance id</param>
    /// <param name="dataGuid">The data id</param>
    /// <param name="request">Http request containing the attachment to be saved</param>
    [Obsolete(
        message: "Deprecated please use UpdateBinaryData(InstanceIdentifier, string, string, Guid, Stream) instead",
        error: false
    )]
    Task<DataElement> UpdateBinaryData(
        string org,
        string app,
        int instanceOwnerPartyId,
        Guid instanceGuid,
        Guid dataGuid,
        HttpRequest request
    );

    /// <summary>
    /// Method that updates a form attachments to disk/storage and returns the updated data element.
    /// </summary>
    /// <param name="instanceIdentifier">Instance identifier instanceOwnerPartyId and instanceGuid</param>
    /// <param name="contentType">Content type of the updated binary data</param>
    /// <param name="filename">Filename of the updated binary data</param>
    /// <param name="dataGuid">Guid of the data element to update</param>
    /// <param name="stream">Updated binary data</param>
    Task<DataElement> UpdateBinaryData(
        InstanceIdentifier instanceIdentifier,
        string? contentType,
        string? filename,
        Guid dataGuid,
        Stream stream
    );

    /// <summary>
    /// Insert a binary data element.
    /// </summary>
    /// <param name="instanceId">isntanceId = {instanceOwnerPartyId}/{instanceGuid}</param>
    /// <param name="dataType">data type</param>
    /// <param name="contentType">content type</param>
    /// <param name="filename">filename</param>
    /// <param name="stream">the stream to stream</param>
    /// <param name="generatedFromTask">Optional field to set what task the binary data was generated from</param>
    /// <returns></returns>
    Task<DataElement> InsertBinaryData(
        string instanceId,
        string dataType,
        string contentType,
        string? filename,
        Stream stream,
        string? generatedFromTask = null
    );

    /// <summary>
    /// Updates the data element metadata object.
    /// </summary>
    /// <param name="instance">The instance which is not updated</param>
    /// <param name="dataElement">The data element with values to update</param>
    /// <returns>the updated data element</returns>
    Task<DataElement> Update(Instance instance, DataElement dataElement);

    /// <summary>
    /// Lock data element in storage
    /// </summary>
    /// <param name="instanceIdentifier">InstanceIdentifier identifying the instance containing the DataElement to lock</param>
    /// <param name="dataGuid">Id of the DataElement to lock</param>
    /// <returns></returns>
    Task<DataElement> LockDataElement(InstanceIdentifier instanceIdentifier, Guid dataGuid);

    /// <summary>
    /// Unlock data element in storage
    /// </summary>
    /// <param name="instanceIdentifier">InstanceIdentifier identifying the instance containing the DataElement to unlock</param>
    /// <param name="dataGuid">Id of the DataElement to unlock</param>
    /// <returns></returns>
    Task<DataElement> UnlockDataElement(InstanceIdentifier instanceIdentifier, Guid dataGuid);
}
