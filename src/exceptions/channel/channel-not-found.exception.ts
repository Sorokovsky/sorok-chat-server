import { NotFoundException } from "../base/not-found.exception";

export class ChannelNotFoundException extends NotFoundException {
  constructor(key: string, value: object) {
    super("Channel", key, value);
  }
}
