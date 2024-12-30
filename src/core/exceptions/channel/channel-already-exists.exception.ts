import { AlreadyExistsException } from "@exceptions/base/already-exists.exception";
import { ChannelEntity } from "@entities/channel.entity";

export class ChannelAlreadyExistsException extends AlreadyExistsException {
  constructor(key: keyof ChannelEntity, value: any) {
    super("Channel", key, value);
  }
}
