using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP_.NET_Conf_2025.Tools
{
    [McpServerToolType] // Definir una clase como una herramienta del servidor MCP
    public static class DevspaceTool
    {
        private const string BaseUrl = "http://localhost:5250";

        [McpServerTool, Description("Obtener todas las carpetas disponibles de la unidad")] // Definir un método como una herramienta del servidor MCP y una descripción de lo que hace (Buena pratica para guiar al modelo de IA)
        public static async Task<string> getAllFolder(HttpClient httpClient)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener una carpeta por su id")]
        public static async Task<string> getFolderById(
            HttpClient httpClient,
            [Description("id de la carpeta, similar a un valor del tipo Guid (ejemplo: 681d0aa3f03a81ee9f9e53b6)")] string id) // Declaramos que este metodo recibe un parametro del tipo string llamado id que lo tiene pasar el modelo de IA
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/{id}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener la carpeta {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener una o varias carpetas por el id de la carpeta padre, si se quiere buscar por las subcarpetas a partir de una id")]
        public static async Task<string> getSubFoldersById(
            HttpClient httpClient,
            [Description("id de la carpeta padre si se quiere buscar las subcarpetas, similar a un valor del tipo Guid (ejemplo: 681d0aa3f03a81ee9f9e53b6)")] string idParent)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/parent/{idParent}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener las subcarpetas {e.Message}";
            }
        }

        [McpServerTool, Description("Buscar carpetas por el nombre")]
        public static async Task<string> getFolderByName(
            HttpClient httpClient,
            [Description("Buscar carpetas por el nombre")] string name)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/name/{name}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener las carpetas por nombre {e.Message}";
            }
        }

        [McpServerTool, Description("Crear una nueva carpeta")]
        public static async Task<string> addFolder(
            HttpClient httpClient,
            [Description("Nombre de la carpeta ")] string name,
            [Description("id de la carpeta padre, puede tener (la carpeta esta dentro de otra carpeta) o no (va en la raiz de la unidad, null)")] string? idParent)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder";
                var folderData = new
                {
                    Name = name,
                    IdParent = idParent
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(folderData), Encoding.UTF8, "application/json"); // Serializar el objeto folderData a JSON
                var response = await httpClient.PostAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al crear la carpeta {e.Message}";
            }
        }

        [McpServerTool, Description("Actualizar el nombre de una carpeta")]
        public static async Task<string> updateFolderName(
            HttpClient httpClient,
            [Description("id de la carpeta a actualizar")] string id,
            [Description("Nuevo nombre de la carpeta")] string name)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/{id}";
                var folderData = new
                {
                    Name = name
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(folderData), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                return "Carpeta actualizada correctamente";
            }
            catch (Exception e)
            {
                return $"Error al actualizar la carpeta {e.Message}";
            }
        }

        [McpServerTool, Description("Actualizar la referencia del ParentFolderId de una carpeta")]
        public static async Task<string> updateFolderParent(
            HttpClient httpClient,
            [Description("id de la carpeta a actualizar")] string id,
            [Description("id de la nueva carpeta padre, puede ser null para moverla a la raiz")] string? parentFolderId)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/parent/{id}";
                var folderData = new
                {
                    ParentFolderID = parentFolderId
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(folderData), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                return "Referencia de carpeta padre actualizada correctamente";
            }
            catch (Exception e)
            {
                return $"Error al actualizar la referencia de carpeta padre {e.Message}";
            }
        }

        [McpServerTool, Description("Eliminar una carpeta y sus subcarpetas")]
        public static async Task<string> deleteFolder(
            HttpClient httpClient,
            [Description("id de la carpeta a eliminar")] string id)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/{id}";

                var response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                return "Carpeta eliminada correctamente";
            }
            catch (Exception e)
            {
                return $"Error al eliminar la carpeta {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener los ids de las subcarpetas de una carpeta especifica")]
        public static async Task<string> getSubFoldersIds(
            HttpClient httpClient,
            [Description("id de la carpeta para obtener sus subcarpetas")] string id)
        {
            try
            {
                var url = $"{BaseUrl}/api/folder/subfolders/{id}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener las subcarpetas {e.Message}";
            }
        }

        // ===== RECURSOS =====

        [McpServerTool, Description("Obtener todos los recursos disponibles")]
        public static async Task<string> getAllResources(HttpClient httpClient)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener un recurso especifico por su id")]
        public static async Task<string> getResourceById(
            HttpClient httpClient,
            [Description("id del recurso")] string id)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/{id}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener el recurso {e.Message}";
            }
        }

        [McpServerTool, Description("Crear un nuevo recurso. Type: 0=Url, 1=Code, 2=Text")]
        public static async Task<string> addResource(
            HttpClient httpClient,
            [Description("Nombre del recurso")] string name,
            [Description("Descripción del recurso")] string description,
            [Description("Tipo de recurso: 0=Url, 1=Code, 2=Text")] int type,
            [Description("Contenido del recurso (URL, código o texto)")] string value,
            [Description("id de la carpeta donde se guardará el recurso, puede ser null para la raiz")] string? folderId = null,
            [Description("Lenguaje de código, requerido solo si type=1 (Code)")] string? codeType = null)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource";
                var resourceData = new
                {
                    FolderId = folderId,
                    Name = name,
                    Description = description,
                    Type = type,
                    CodeType = codeType,
                    Value = value
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(resourceData), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al crear el recurso {e.Message}";
            }
        }

        [McpServerTool, Description("Actualizar un recurso completamente")]
        public static async Task<string> updateResource(
            HttpClient httpClient,
            [Description("id del recurso a actualizar")] string id,
            [Description("Nuevo nombre del recurso")] string name,
            [Description("Nueva descripción del recurso")] string description,
            [Description("Nuevo contenido del recurso")] string value)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/{id}";
                var resourceData = new
                {
                    Name = name,
                    Description = description,
                    Value = value
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(resourceData), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                return "Recurso actualizado correctamente";
            }
            catch (Exception e)
            {
                return $"Error al actualizar el recurso {e.Message}";
            }
        }

        [McpServerTool, Description("Actualizar el folderId de un recurso")]
        public static async Task<string> updateResourceFolderId(
            HttpClient httpClient,
            [Description("id del recurso a actualizar")] string id,
            [Description("Nuevo id de carpeta, puede ser null para moverlo a la raiz")] string? folderId)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/folderid/{id}";
                var resourceData = new
                {
                    FolderId = folderId
                };

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(resourceData), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                return "FolderId del recurso actualizado correctamente";
            }
            catch (Exception e)
            {
                return $"Error al actualizar el folderId del recurso {e.Message}";
            }
        }

        [McpServerTool, Description("Eliminar un recurso")]
        public static async Task<string> deleteResource(
            HttpClient httpClient,
            [Description("id del recurso a eliminar")] string id)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/{id}";

                var response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                return "Recurso eliminado correctamente";
            }
            catch (Exception e)
            {
                return $"Error al eliminar el recurso {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener recursos por nombre")]
        public static async Task<string> getResourcesByName(
            HttpClient httpClient,
            [Description("Nombre del recurso a buscar")] string name)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/name/{name}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos por nombre {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener recursos favoritos")]
        public static async Task<string> getFavoriteResources(HttpClient httpClient)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/favorites";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos favoritos {e.Message}";
            }
        }

        [McpServerTool, Description("Alternar el estado de favorito de un recurso")]
        public static async Task<string> toggleResourceFavorite(
            HttpClient httpClient,
            [Description("id del recurso")] string id)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/favorite/{id}";

                var response = await httpClient.PutAsync(url, null);
                response.EnsureSuccessStatusCode();

                return "Estado de favorito actualizado correctamente";
            }
            catch (Exception e)
            {
                return $"Error al actualizar el estado de favorito {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener recursos por folderId")]
        public static async Task<string> getResourcesByFolderId(
            HttpClient httpClient,
            [Description("id de la carpeta")] string folderId)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/folder/{folderId}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos por folderId {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener recursos de la raiz (sin carpeta padre)")]
        public static async Task<string> getRootResources(HttpClient httpClient)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/root";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos raiz {e.Message}";
            }
        }

        [McpServerTool, Description("Obtener recursos recientes")]
        public static async Task<string> getRecentResources(HttpClient httpClient)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/recents";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (Exception e)
            {
                return $"Error al obtener los recursos recientes {e.Message}";
            }
        }

        [McpServerTool, Description("Eliminar todos los recursos de una carpeta")]
        public static async Task<string> deleteResourcesByFolderId(
            HttpClient httpClient,
            [Description("id de la carpeta")] string folderId)
        {
            try
            {
                var url = $"{BaseUrl}/api/resource/folder/{folderId}";

                var response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                return "Recursos de la carpeta eliminados correctamente";
            }
            catch (Exception e)
            {
                return $"Error al eliminar los recursos de la carpeta {e.Message}";
            }
        }
    }
}
