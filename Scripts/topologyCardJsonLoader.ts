import { ITopologyCardJsonLoader } from './ITopologyCardJsonLoader.js';
import { TopologyCardJson } from './JsonType.js';

export class TopologyCardJsonLoader implements ITopologyCardJsonLoader {

    /**
     * トポロジーカードをjsonから読み込む
     * @param url jsonが配置されているurl
     * @returns jsonファイルをTopologyCardJsonタイプの配列に変換した結果
     */
    loadTopologyCardsJson(url: string): TopologyCardJson[] {
        let result: TopologyCardJson[];
        $.ajax({
            url: url,
            dataType: "json",
            async: false,
            success: function (data) {
                result = data as TopologyCardJson[];
            }
        });

        return result;
    }
}