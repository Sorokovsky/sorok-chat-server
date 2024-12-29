import { NotFoundException } from "../base/not-found.exception";
import { ChannelEntity } from "../../entities/channel.entity";

export class ChannelNotFoundException extends NotFoundException {
  constructor(key: keyof ChannelEntity, value: any) {
    super("Channel", key, value);
  }
}
