export class TopologyCardJsonLoader {
    /**
     * トポロジーカードをjsonから読み込む
     * @param url jsonが配置されているurl
     * @returns jsonファイルをTopologyCardJsonタイプの配列に変換した結果
     */
    loadTopologyCardsJson(url) {
        let result;
        $.ajax({
            url: url,
            dataType: "json",
            async: false,
            success: function (data) {
                result = data;
            }
        });
        return result;
    }
}
