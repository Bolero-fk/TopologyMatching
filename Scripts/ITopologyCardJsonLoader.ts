import { TopologyCardJson } from './JsonType.js';

export interface ITopologyCardJsonLoader {
    loadTopologyCardsJson(url: string): TopologyCardJson[];
}
