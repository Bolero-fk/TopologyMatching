import { TopologyCardJson } from '../dist/JsonType.js';

export interface ITopologyCardJsonLoader {
    loadTopologyCardsJson(url: string): TopologyCardJson[];
}
