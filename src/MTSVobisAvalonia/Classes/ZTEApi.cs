using System;
using System.Collections.Generic;
using System.Text;

namespace MTSVobisAvalonia.Classes
{
    public class ZTEApi
    {
        public const string GET_SMS_CAPACITY_INFO = "/goform/goform_get_cmd_process?cmd=sms_capacity_info";
        public const string GET_SMS_DATA = "/goform/goform_get_cmd_process?cmd=sms_data_total";
        public const string SET_SMS_READED = "/goform/goform_set_cmd_process?goformId=SET_MSG_READ";
        public const string SET_DELETE_SMS = "/goform/goform_set_cmd_process?goformId=DELETE_SMS";

        public const string GET_PERIODICAL_DEVICE_STATUS = 
            "/goform/goform_get_cmd_process?multi_data=1&sms_received_flag_flag=0&sts_received_flag_flag=0&cmd=" +
            "modem_main_state," +
            "pin_status," +
            "blc_wan_mode," +
            "blc_wan_auto_mode," +
            "loginfo," +
            "fota_new_version_state," +
            "fota_current_upgrade_state," +
            "fota_upgrade_selector," +
            "network_provider," +
            "is_mandatory," +
            "sta_count," +
            "m_sta_count," +
            "signalbar," +
            "network_type," +
            "sub_network_type," +
            "ppp_status," +
            "rj45_state," +
            "EX_SSID1," +
            "sta_ip_status," +
            "EX_wifi_profile," +
            "m_ssid_enable," +
            "wifi_cur_state," +
            "SSID1,simcard_roam," +
            "lan_ipaddr," +
            "battery_charging," +
            "battery_vol_percent," +
            "battery_pers," +
            "spn_name_data," +
            "spn_b1_flag," +
            "spn_b2_flag," +
            "realtime_tx_bytes," +
            "realtime_rx_bytes," +
            "realtime_time," +
            "realtime_tx_thrpt," +
            "realtime_rx_thrpt," +
            "monthly_rx_bytes," +
            "monthly_tx_bytes," +
            "traffic_alined_delta," +
            "monthly_time," +
            "date_month," +
            "data_volume_limit_switch," +
            "data_volume_limit_size," +
            "data_volume_alert_percent," +
            "data_volume_limit_unit," +
            "roam_setting_option," +
            "upg_roam_switch," +
            "fota_package_already_download," +
            "ssid," +
            "dial_mode," +
            "ethwan_mode," +
            "default_wan_name," +
            "sms_received_flag," +
            "sts_received_flag," +
            "sms_unread_num," +
            "rssi," +
            "sim_imsi," +
            "wan_ipaddr," +
            "ipv6_wan_ipaddr," +
            "imei";
    }
}
